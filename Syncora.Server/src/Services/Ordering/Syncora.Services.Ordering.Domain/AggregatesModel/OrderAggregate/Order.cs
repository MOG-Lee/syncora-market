using Syncora.Services.Ordering.Domain.Events;
using Syncora.SharedKernel.Domain;
using Syncora.SharedKernel.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncora.Services.Ordering.Domain.AggregatesModel.OrderAggregate
{
    public class Order : Entity, IAggregateRoot
    {
        private readonly List<OrderItem> _orderItems = new();

        public DateTime OrderDate { get; private set; }
        public Guid BuyerId { get; private set; }
        public Address ShippingAddress { get; private set; } = default!;
        public OrderStatus OrderStatus { get; private set; }
        public decimal Total { get; private set; }
        public IReadOnlyCollection<OrderItem> OrderItems => _orderItems.AsReadOnly();

        // for EF Core
        protected Order()
        {
            _orderItems = new List<OrderItem>();
        }

        // private constructor to force usage of the Factory
        private Order(Guid buyerId, Address shippingAddress) : this()
        {
            if (buyerId == Guid.Empty)
                throw new DomainException("Buyer ID cannot be empty");

            if (shippingAddress == null)
                throw new DomainException("Shipping address cannot be null");

            BuyerId = buyerId;
            ShippingAddress = shippingAddress;
            OrderDate = DateTime.UtcNow;
            OrderStatus = OrderStatus.Submitted;
        }

        public static Order Create(Guid buyerId, Address shippingAddress, List<OrderItemCandidate> orderItems)
        {
            if (orderItems == null || !orderItems.Any())
            {
                throw new DomainException("Cannot create an order with zero items.");
            }

            var order = new Order(buyerId, shippingAddress);

            foreach (var item in orderItems)
            {
                order.AddOrderItem(item.ProductId, item.ProductName, item.UnitPrice, item.PictureUrl,
                                   item.Units
                );
            }

            order.AddDomainEvent(new OrderStartedDomainEvent(order));
            return order;
        }

        // Tell, Do not Ask principle
        public void AddOrderItem(Guid productId, string productName, decimal unitPrice, string pictureUrl, int units = 1)
        {
            var existingOrderItem = _orderItems.FirstOrDefault(oi => oi.ProductId == productId);
            if (existingOrderItem != null)
            {
                existingOrderItem.AddUnits(units);
            }
            else
            {
                var orderItem = new OrderItem(productId, productName, unitPrice, pictureUrl, units);
                _orderItems.Add(orderItem);
            }
            CalculateTotal();
        }

        public void SetCancelled()
        {
            if (OrderStatus == OrderStatus.Shipped)
                throw new DomainException("Cannot cancel an order that has already been shipped.");

            OrderStatus = OrderStatus.Cancelled;

            AddDomainEvent(new OrderCancelledDomainEvent(this));
        }

        private void CalculateTotal()
        {
            Total = _orderItems.Sum(oi => oi.UnitPrice * oi.Units);
        }
    }
}
