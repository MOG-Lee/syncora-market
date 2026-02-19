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
    public class StockRejectedConsumer : IConsumer<StockRejectedIntegrationEvent>
    {
        private readonly IOrderRepository _repository;
        private readonly ILogger<StockRejectedConsumer> _logger;

        public StockRejectedConsumer(IOrderRepository repository, ILogger<StockRejectedConsumer> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<StockRejectedIntegrationEvent> context)
        {
            _logger.LogWarning("Processing Stock Rejection for Order {OrderId}. Reason: {Reason}",
                        context.Message.OrderId, context.Message.Reason);

            try
            {
                var order = await _repository.GetAsync(context.Message.OrderId);
                if (order == null) return;

                try
                {
                    order.SetStockRejected(context.Message.Reason);
                }
                catch (DomainException ex)
                {
                    _logger.LogWarning("Business rule violation for Order {OrderId}: {Message}", context.Message.OrderId, ex.Message);
                    return; // Do not retry
                }

                await _repository.UnitOfWork.SaveEntitiesAsync(context.CancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "System failure rejecting stock for Order {OrderId}", context.Message.OrderId);
                throw;
            }
        }
    }
}
