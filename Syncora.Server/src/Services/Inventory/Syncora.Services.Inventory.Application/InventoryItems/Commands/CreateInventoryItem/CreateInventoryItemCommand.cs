using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncora.Services.Inventory.Application.InventoryItems.Commands.CreateInventoryItem
{
    public record CreateInventoryItemCommand(Guid ProductId, int AvailableStock) : IRequest<Guid>;
}
