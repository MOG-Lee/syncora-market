using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Syncora.Services.Ordering.Application.Orders.Commands.CancelOrder;
using Syncora.Services.Ordering.Application.Orders.Commands.CreateOrder;
using Syncora.Services.Ordering.Application.Orders.Queries.GetOrderDetails;
using Syncora.Services.Ordering.Application.Orders.Queries.GetOrderSummary;
using System.Security.Claims;

namespace Syncora.Services.Ordering.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;
        public OrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // POST api/orders
        [HttpPost]
        //[ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PlaceOrder([FromBody] CreateOrderRequest request)
        {
            var buyerId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var command = new CreateOrderCommand(buyerId, request.ShippingAddress, request.OrderItems);

            var orderId = await _mediator.Send(command);

            return Ok(orderId);
        }

        // GET api/orders/{id}
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(OrderDetailDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetOrder(Guid id)
        {
            var query = new GetOrderDetailsQuery(id);
            var order = await _mediator.Send(query);

            return Ok(order);
        }

        // GET api/orders/customer/
        [HttpGet("customer")]
        [ProducesResponseType(typeof(IEnumerable<OrderSummaryDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetOrdersByCustomer()
        {
            var buyerId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var query = new GetOrderSummaryQuery(buyerId);
            var orders = await _mediator.Send(query);
            return Ok(orders);
        }

        // PUT api/orders/{id}/cancel
        [HttpPut("{id}/cancel")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CancelOrder(Guid id)
        {
            var command = new CancelOrderCommand(id);
            await _mediator.Send(command);
            return NoContent();
        }
    }

    public record CreateOrderRequest(CreateOrderAddressDto ShippingAddress, List<CreateOrderOrderItemDto> OrderItems);
}
