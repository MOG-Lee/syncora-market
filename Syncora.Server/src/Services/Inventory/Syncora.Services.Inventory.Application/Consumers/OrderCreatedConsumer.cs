using MassTransit;
using Microsoft.Extensions.Logging;
using Syncora.Services.Inventory.Domain.AggregatesModel.InventoryAggregate;
using Syncora.SharedKernel.Exceptions;
using Syncora.SharedKernel.IntegrationEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncora.Services.Inventory.Application.Consumers
{
    public class OrderCreatedConsumer : IConsumer<OrderCreatedIntegrationEvent>
    {
        private readonly IInventoryRepository _repository;
        private readonly ILogger<OrderCreatedConsumer> _logger;

        public OrderCreatedConsumer(IInventoryRepository inventoryRepository, ILogger<OrderCreatedConsumer> logger)
        {
            _repository = inventoryRepository;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<OrderCreatedIntegrationEvent> context)
        {
            _logger.LogInformation("Checking stock for order {OrderId}", context.Message.OrderId);

            try
            {
                foreach (var item in context.Message.Items)
                {
                    var inventoryItem = await _repository.GetByProductIdAsync(item.ProductId);

                    if (inventoryItem == null)
                    {
                        _logger.LogWarning("Order {OrderId} Rejected: Product {ProductId} not found.", context.Message.OrderId, item.ProductId);
                        await RejectOrder(context, $"Product {item.ProductId} not found");
                        return;
                    }

                    if (inventoryItem.AvailableStock < item.Units)
                    {
                        _logger.LogWarning("Order {OrderId} Rejected: Insufficient stock for {ProductId}.", context.Message.OrderId, item.ProductId);
                        await RejectOrder(context, $"Insufficient stock for {item.ProductId}");
                        return;
                    }
                }

                foreach (var item in context.Message.Items)
                {
                    var inventoryItem = await _repository.GetByProductIdAsync(item.ProductId);
                    inventoryItem!.RemoveStock(item.Units);
                }

                await context.Publish(new StockConfirmedIntegrationEvent(context.Message.OrderId));

                _logger.LogInformation("Stock successfully reserved for Order {OrderId}", context.Message.OrderId);

                await _repository.UnitOfWork.SaveEntitiesAsync(context.CancellationToken);
            }
            catch (CustomConcurrencyException ex)
            {
                //concurrency can event happens for stock = 50. So, we reporocess on the next try.
                _logger.LogWarning(ex, "Concurrency conflict detected for Order {OrderId}. MassTransit will retry the message to re-evaluate stock.", context.Message.OrderId);
                throw;
            }
            catch (Exception ex)
            {
                //system failures
                _logger.LogError(ex, "System failure processing Order {OrderId}", context.Message.OrderId);
                throw;
            }
        }

        // Helper to publish rejection and save the outbox message
        private async Task RejectOrder(ConsumeContext<OrderCreatedIntegrationEvent> context, string reason)
        {
            await context.Publish(new StockRejectedIntegrationEvent(context.Message.OrderId, reason));

            // Save messages in Outbox (part of the same transaction)
            await _repository.UnitOfWork.SaveEntitiesAsync(context.CancellationToken);
        }
    }
}
