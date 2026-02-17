using MassTransit;
using MediatR;
using Syncora.Services.Ordering.Domain.Events;
using Syncora.SharedKernel.IntegrationEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncora.Services.Ordering.Application.Orders.Events
{
    public class OrderStartedDomainEventHandler : INotificationHandler<OrderStartedDomainEvent>
    {
        private readonly IPublishEndpoint _publishEndpoint;
        public OrderStartedDomainEventHandler(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }


        public async Task Handle(OrderStartedDomainEvent notification, CancellationToken cancellationToken)
        {
            var integrationEvent = new OrderCreatedIntegrationEvent(
                notification.Order.Id,
                notification.Order.BuyerId,
                notification.Order.Total,
                notification.Order.OrderItems.Select(x => new OrderItemDto(x.ProductId, x.Units)).ToList()
            );

            // Publish to the "Outbox" (not directly to RabbitMQ yet)
            await _publishEndpoint.Publish(integrationEvent, cancellationToken);
        }
    }
}
