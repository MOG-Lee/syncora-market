using MassTransit;
using Microsoft.Extensions.Logging;
using Syncora.Services.Ordering.Domain.AggregatesModel.OrderAggregate;
using Syncora.SharedKernel.Exceptions;
using Syncora.SharedKernel.IntegrationEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncora.Services.Ordering.Application.Consumers
{
    public class StockConfirmedConsumer : IConsumer<StockConfirmedIntegrationEvent>
    {
        private readonly IOrderRepository _repository;
        private readonly ILogger<StockConfirmedConsumer> _logger;
        public StockConfirmedConsumer(IOrderRepository repository, ILogger<StockConfirmedConsumer> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<StockConfirmedIntegrationEvent> context)
        {
            _logger.LogInformation("Processing Stock Confirmation for Order {OrderId}", context.Message.OrderId);

            try
            {
                var order = await _repository.GetAsync(context.Message.OrderId);
                if (order == null) 
                    return;

                try
                {
                    order.SetStockConfirmed();
                }
                catch (DomainException ex)
                {
                    // The order was likely cancelled by the user while Inventory was processing.
                    // maybe we should fire OrderCancelledIntegrationEvent here
                    _logger.LogWarning("Business rule violation for Order {OrderId}: {Message}", context.Message.OrderId, ex.Message);

                    // Swallow message. Do not retry.
                    return;
                }

                await _repository.UnitOfWork.SaveEntitiesAsync(context.CancellationToken);
                _logger.LogInformation("Order {OrderId} status updated to StockConfirmed.", context.Message.OrderId);
            }
            catch (Exception ex)
            {
                // SYSTEM LOGIC (DB Down, etc) -> Rethrow!
                _logger.LogError(ex, "System failure confirming stock for Order {OrderId}", context.Message.OrderId);
                throw;
            }
        }
    }
}
