
using Microsoft.EntityFrameworkCore;
using Syncora.Services.Inventory.Domain.AggregatesModel.InventoryAggregate;
using Syncora.Services.Inventory.Infrastructure.Persistence;
using Syncora.SharedKernel.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncora.Services.Inventory.Infrastructure.Repositories
{
    public class InventoryRepository : IInventoryRepository
    {
        private readonly InventoryDbContext _context;
        public InventoryRepository(InventoryDbContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        public InventoryItem Add(InventoryItem inventoryItem)
        {
            return _context.InventoryItems.Add(inventoryItem).Entity;
        }

        public async Task<InventoryItem?> GetByProductIdAsync(Guid productId)
        {
            return await _context.InventoryItems
            .FirstOrDefaultAsync(x => x.ProductId == productId);
        }
    }
}
