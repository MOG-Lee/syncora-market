using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Syncora.Services.Inventory.Application.InventoryItems.Commands.CreateInventoryItem;
using Syncora.Services.Inventory.Application.InventoryItems.Queries.GetInventoryStock;

namespace Syncora.Services.Inventory.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryItemsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public InventoryItemsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize(Roles = "Admin, SuperAdmin")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateInventoryItem([FromBody] CreateInventoryItemCommand command)
        {
            var inventoryItemId = await _mediator.Send(command);
            return Ok(inventoryItemId);
        }

        // GET api/InventoyItems/{id}
        [HttpGet("{productId}")]
        public async Task<IActionResult> GetInventoryItemAvailableStock(Guid productId)
        {
            var query = new GetInventoryStockQuery(productId);
            var availableStock = await _mediator.Send(query);

            return Ok(availableStock);
        }
    }
}
