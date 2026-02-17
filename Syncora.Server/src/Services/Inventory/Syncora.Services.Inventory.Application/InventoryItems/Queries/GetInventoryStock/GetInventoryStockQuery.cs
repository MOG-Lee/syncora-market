using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncora.Services.Inventory.Application.InventoryItems.Queries.GetInventoryStock
{
    public record GetInventoryStockQuery(Guid ProductId) : IRequest<InventoryStockDto>;

    public record InventoryStockDto
    {
        public int AvailableStock { get; init; }
    }
}
