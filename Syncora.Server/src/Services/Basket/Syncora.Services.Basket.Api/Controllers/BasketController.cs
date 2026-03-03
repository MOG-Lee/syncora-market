using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Syncora.Services.Basket.Application.Interfaces;
using Syncora.Services.Basket.Domain.Entities;
using System.Security.Claims;

namespace Syncora.Services.Basket.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _repository;
        private readonly ILogger<BasketController> _logger;
        public BasketController(IBasketRepository repository, ILogger<BasketController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        // GET api/basket
        [HttpGet]
        [ProducesResponseType(typeof(CustomerBasket), StatusCodes.Status200OK)]
        public async Task<ActionResult<CustomerBasket>> GetBasket()
        {
            var buyerId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var basket = await _repository.GetBasketAsync(buyerId);

            return Ok(basket ?? new CustomerBasket(buyerId));
        }

        [HttpPost]
        [ProducesResponseType(typeof(CustomerBasket), StatusCodes.Status200OK)]
        public async Task<ActionResult<CustomerBasket>> UpdateBasket([FromBody] CustomerBasket basket)
        {
            basket.BuyerId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var updatedBasket = await _repository.UpdateBasketAsync(basket);
            return Ok(updatedBasket);
        }

        [HttpDelete]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> DeleteBasketById()
        {
            var buyerId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var result = await _repository.DeleteBasketAsync(buyerId);
            return Ok(result);
        }
    }
}
