using MediatR;
using Syncora.Services.Ordering.Domain.AggregatesModel.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncora.Services.Ordering.Application.Orders.Commands.CreateOrder
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Guid>
    {
        private readonly IOrderRepository _orderRepository;

        public CreateOrderCommandHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<Guid> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            // if we need more data like checking real price of products, we can 1) http get to catalog (ICatalogIntegrationService) 2) duplicated data and use IOrderingProductRepository
            // if product prices are not the same, we can throw DomainException 
            // we do not pass whole user entity to the order for logics, we only send properties which needed like buyerDetails.IsTaxExempt

            var shippingAddress = new Address(request.ShippingAddress.Street, request.ShippingAddress.City, request.ShippingAddress.ZipCode);

            var itemsCandidate = request.OrderItems.Select(i => new OrderItemCandidate(
                i.ProductId, i.ProductName, i.UnitPrice, i.PictureUrl, i.Units)).ToList();

            var order = Order.Create(request.BuyerId, shippingAddress, itemsCandidate);

            _orderRepository.Add(order);

            await _orderRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

            return order.Id;
        }
    }
}
