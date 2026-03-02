using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Syncora.Services.Basket.Application.Interfaces;
using Syncora.Services.Basket.Domain.Entities;

namespace Syncora.Services.Basket.Api.Controllers
{
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

        // GET api/basket/{buyerId}
        [HttpGet("{buyerId:guid}")]
        [ProducesResponseType(typeof(CustomerBasket), StatusCodes.Status200OK)]
        public async Task<ActionResult<CustomerBasket>> GetBasketById(Guid buyerId)
        {
            var basket = await _repository.GetBasketAsync(buyerId);

            return Ok(basket ?? new CustomerBasket(buyerId));
        }

        [HttpPost]
        [ProducesResponseType(typeof(CustomerBasket), StatusCodes.Status200OK)]
        public async Task<ActionResult<CustomerBasket>> UpdateBasket([FromBody] CustomerBasket basket)
        {
            var updatedBasket = await _repository.UpdateBasketAsync(basket);
            return Ok(updatedBasket);
        }

        [HttpDelete("{buyerId:guid}")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> DeleteBasketById(Guid buyerId)
        {
            var result = await _repository.DeleteBasketAsync(buyerId);
            return Ok(result);
        }
    }
}
