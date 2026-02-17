using Syncora.SharedKernel.Domain;
using Syncora.SharedKernel.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncora.Services.Inventory.Domain.AggregatesModel.InventoryAggregate
{
    public class InventoryItem : Entity, IAggregateRoot
    {
        public Guid ProductId { get; private set; }
        public int AvailableStock { get; private set; }
        public byte[] RowVersion { get; private set; }

        private InventoryItem() { }

        private InventoryItem(Guid productId, int availableStock)
        {
            if (productId == Guid.Empty)
                throw new DomainException("Product ID cannot be empty");

            if (availableStock < 0)
                throw new DomainException("Available stock cannot be negative");

            ProductId = productId;
            AvailableStock = availableStock;
        }

        public static InventoryItem Create(Guid productId, int initialStock)
        {
            var inventoryItem = new InventoryItem(productId, initialStock);

            return inventoryItem;
        }

        public void AddStock(int quantity)
        {
            if (quantity <= 0)
                throw new DomainException("Quantity to add must be greater than zero");

            AvailableStock += quantity;
        }

        public void RemoveStock(int quantity)
        {
            if (quantity <= 0)
                throw new DomainException("Quantity to remove must be greater than zero");

            if (AvailableStock < quantity)
                throw new DomainException("Not enough stock available to remove the specified quantity");

            AvailableStock -= quantity;
        }
    }
}
