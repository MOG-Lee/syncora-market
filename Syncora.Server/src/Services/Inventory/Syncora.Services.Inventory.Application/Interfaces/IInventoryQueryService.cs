using Syncora.Services.Inventory.Application.InventoryItems.Queries.GetInventoryStock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncora.Services.Inventory.Application.Interfaces
{
    public interface IInventoryQueryService
    {
        Task<InventoryStockDto> GetInventoryStockAsync(Guid productId);
    }
}
