using Syncora.SharedKernel.Domain;
using Syncora.SharedKernel.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncora.Services.Ordering.Domain.AggregatesModel.OrderAggregate
{
    public class OrderItem : Entity
    {
        public string ProductName { get; private set; } = default!;
        public string PictureUrl { get; private set; } = default!;
        public decimal UnitPrice { get; private set; }
        public int Units { get; private set; }
        public Guid ProductId { get; private set; }

        protected OrderItem() { }

        internal OrderItem(Guid productId, string productName, decimal unitPrice, string pictureUrl, int units = 1)
        {
            if (string.IsNullOrWhiteSpace(productName))
                throw new DomainException("Product name cannot be null or empty");

            if (unitPrice <= 0)
                throw new DomainException("Unit price must be greater than zero");

            if (units <= 0)
                throw new DomainException("Units must be greater than zero");

            if (productId == Guid.Empty)
                throw new DomainException("Product ID cannot be empty");

            ProductName = productName;
            UnitPrice = unitPrice;
            Units = units;
            ProductId = productId;
            PictureUrl = pictureUrl;
        }

        // Tell, Do not Ask principle
        // not doing item.Units += unitsToAdd; in the Order aggregate
        public void AddUnits(int units)
        {
            if (units <= 0)
                throw new DomainException("Units to add must be greater than zero");

            Units += units;
        }
    }
}
