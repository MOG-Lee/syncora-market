using MediatR;
using Syncora.Services.Inventory.Domain.AggregatesModel.InventoryAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncora.Services.Inventory.Application.InventoryItems.Commands.CreateInventoryItem
{
    public class CreateInventoryItemCommandHandler : IRequestHandler<CreateInventoryItemCommand, Guid>
    {
        private readonly IInventoryRepository _inventoryRepository;

        public CreateInventoryItemCommandHandler(IInventoryRepository inventoryRepository)
        {
            _inventoryRepository = inventoryRepository;
        }

        public async Task<Guid> Handle(CreateInventoryItemCommand request, CancellationToken cancellationToken)
        {
            var inventoryItem = InventoryItem.Create(request.ProductId, request.AvailableStock);

            _inventoryRepository.Add(inventoryItem);

            await _inventoryRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
            return inventoryItem.Id;    
        }
    }
}
