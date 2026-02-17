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
            try
            {
                _logger.LogInformation("Restocking items for Cancelled Order {OrderId}", context.Message.OrderId);

                foreach (var item in context.Message.items)
                {
                    var inventoryItem = await _repository.GetByProductIdAsync(item.ProductId);

                    if (inventoryItem != null)
                    {
                        inventoryItem.AddStock(item.Units);
                    }
                }
                await _repository.UnitOfWork.SaveEntitiesAsync(context.CancellationToken);
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
