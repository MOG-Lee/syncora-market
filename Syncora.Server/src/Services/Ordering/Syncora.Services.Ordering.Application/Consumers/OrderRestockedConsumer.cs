using MassTransit;
using Microsoft.Extensions.Logging;
using Syncora.Services.Ordering.Domain.AggregatesModel.OrderAggregate;
using Syncora.SharedKernel.IntegrationEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncora.Services.Ordering.Application.Consumers
{
    public class OrderRestockedConsumer : IConsumer<OrderRestockedIntegrationEvent>
    {
        private readonly IOrderRepository _repository;
        private readonly ILogger<OrderRestockedConsumer> _logger;
        public OrderRestockedConsumer(IOrderRepository repository, ILogger<OrderRestockedConsumer> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<OrderRestockedIntegrationEvent> context)
        {
            _logger.LogInformation("Order {OrderId} items have been restocked in Inventory.", context.Message.OrderId);

            // maybe fire some other events

            await Task.CompletedTask;
        }
    }
}
