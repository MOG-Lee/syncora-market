using Syncora.SharedKernel.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncora.Services.Inventory.Domain.AggregatesModel.InventoryAggregate
{
    public interface IInventoryRepository
    {
        IUnitOfWork UnitOfWork { get; }
        InventoryItem Add(InventoryItem inventoryItem);
        Task<InventoryItem?> GetByProductIdAsync(Guid productId);
    }
}
