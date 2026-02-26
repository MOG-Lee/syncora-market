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
    public class OrderCancelledConsumer : IConsumer<OrderCancelledIntegrationEvent>
    {
        private readonly IInventoryRepository _repository;
        private readonly ILogger<OrderCancelledConsumer> _logger;

        public OrderCancelledConsumer(IInventoryRepository repository, ILogger<OrderCancelledConsumer> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<OrderCancelledIntegrationEvent> context)
        {
            _logger.LogInformation("Restocking items for Cancelled Order {OrderId}", context.Message.OrderId);

            try
            {
                foreach (var item in context.Message.Items)
                {
                    var inventoryItem = await _repository.GetByProductIdAsync(item.ProductId);

                    if (inventoryItem != null)
                    {
                        inventoryItem.AddStock(item.Units);
                    }
                    else
                    {
                        // Just log warning, don't crash.
                        _logger.LogWarning("Product {ProductId} not found during restock for Order {OrderId}", item.ProductId, context.Message.OrderId);
                    }
                }

                // 2. Publish Confirmation (Saga Step) and save in outbox table
                await context.Publish(new OrderRestockedIntegrationEvent(context.Message.OrderId));

                await _repository.UnitOfWork.SaveEntitiesAsync(context.CancellationToken);

                _logger.LogInformation("Order {OrderId} successfully restocked.", context.Message.OrderId);
            }
            catch (Exception ex)
            {
                // system failures like Database is down, Network blip
                _logger.LogError(ex, "System failure during restock for Order {OrderId}", context.Message.OrderId);

                // re-throw so MassTransit keeps the message in RabbitMQ 
                throw;
            }
        }
    }
}
