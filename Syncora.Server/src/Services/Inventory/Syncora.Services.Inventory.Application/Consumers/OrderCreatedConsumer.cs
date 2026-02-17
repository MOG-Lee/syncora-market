using MassTransit;
using Microsoft.Extensions.Logging;
using Syncora.Services.Inventory.Domain.AggregatesModel.InventoryAggregate;
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
            try
            {
                _logger.LogInformation("Checking stock for order {OrderId}", context.Message.OrderId);

                foreach (var item in context.Message.items)
                {
                    var inventoryItem = await _repository.GetByProductIdAsync(item.ProductId);

                    if (inventoryItem == null)
                    {
                        // TODO: Publish StockRejectedEvent (Product doesn't exist)
                        _logger.LogWarning("Product {ProductId} not found in inventory!", item.ProductId);
                        // For now, we return, but this leaves the Order in "Submitted" state forever.
                        // We will fix this when we implement Saga Compensation.
                        //** sth like await context.Publish(new StockRejectedEvent(context.Message.OrderId, "Product not found"));
                        return;
                    }
                    else
                    {
                        try
                        {
                            inventoryItem.RemoveStock(item.Units);
                        }
                        catch (Exception ex)
                        {
                            // TODO: Publish StockRejectedEvent (Insufficient Stock)
                            _logger.LogError("Insufficient stock for Order {OrderId}: {Message}", context.Message.OrderId, ex.Message);
                            //** sth like await context.Publish(new StockRejectedEvent(context.Message.OrderId, "Insufficient stock"));
                            return;
                        }
                    }
                }

                // use ef tracker to save all modified inventory items in one transaction
                await _repository.UnitOfWork.SaveEntitiesAsync();

                _logger.LogInformation("Stock reserved for Order {OrderId}", context.Message.OrderId);

                // TODO: Publish StockConfirmedIntegrationEvent (so Ordering service can change status to 'StockConfirmed')
                // sth like await context.Publish(new StockConfirmedIntegrationEvent(context.Message.OrderId));
            }
            catch (Exception ex)
            {
                //system crash. re-throw so MassTransit knows to Retry 
                _logger.LogError(ex, "System failure while saving inventory for Order {OrderId}", context.Message.OrderId);
                throw;
            }
        }
    }
}
