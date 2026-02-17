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
    public class OrderCancelledDomainEventHandler : INotificationHandler<OrderCancelledDomainEvent>
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public OrderCancelledDomainEventHandler(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public async Task Handle(OrderCancelledDomainEvent notification, CancellationToken cancellationToken)
        {
            var integrationEvent = new OrderCancelledIntegrationEvent
                (
                notification.Order.Id,
                notification.Order.OrderItems.Select(i => new OrderCancelledItemDto(i.ProductId, i.Units)).ToList()
                );

            await _publishEndpoint.Publish(integrationEvent, cancellationToken);
        }
    }
}
