using MediatR;
using Syncora.Services.Ordering.Domain.AggregatesModel.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncora.Services.Ordering.Application.Orders.Commands.CancelOrder
{
    public class CancelOrderCommandHandler : IRequestHandler<CancelOrderCommand, bool>
    {
        private readonly IOrderRepository _orderRepository;

        public CancelOrderCommandHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<bool> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetAsync(request.OrderId);

            if (order == null)
            {
                throw new KeyNotFoundException($"Order {request.OrderId} not found.");
            }

            order.SetCancelled();

            await _orderRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

            return true;
        }
    }
}
