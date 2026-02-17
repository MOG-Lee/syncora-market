using MediatR;
using Syncora.Services.Inventory.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncora.Services.Inventory.Application.InventoryItems.Queries.GetInventoryStock
{
    public class GetInventoryStockQueryHandler : IRequestHandler<GetInventoryStockQuery, InventoryStockDto>
    {
        private readonly IInventoryQueryService _queryService;

        public GetInventoryStockQueryHandler(IInventoryQueryService queryService)
        {
            _queryService = queryService;
        }

        public async Task<InventoryStockDto> Handle(GetInventoryStockQuery request, CancellationToken cancellationToken)
        {
            return await _queryService.GetInventoryStockAsync(request.ProductId);
        }
    }
}
