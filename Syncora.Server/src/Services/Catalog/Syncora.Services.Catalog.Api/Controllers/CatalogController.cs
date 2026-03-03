using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Syncora.Services.Catalog.Application.Interfaces;
using Syncora.Services.Catalog.Domain.Entities;

namespace Syncora.Services.Catalog.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly IProductRepository _repository;
        private readonly ILogger<CatalogController> _logger;

        public CatalogController(IProductRepository repository, ILogger<CatalogController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        //Catalog
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var products = await _repository.GetProductsAsync();
            return Ok(products);
        }

        //Catalog/{id}
        [AllowAnonymous]
        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Product>> GetProductById(Guid id)
        {
            var product = await _repository.GetProductAsync(id);
            if (product == null)
            {
                _logger.LogError("Product with id: {Id}, not found.", id);
                throw new KeyNotFoundException($"Product with id: {id}, not found.");
            }
            return Ok(product);
        }

        //Catalog/GetProductByCategory/{category}
        [AllowAnonymous]
        [HttpGet("[action]/{category}")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductByCategory(string category)
        {
            var products = await _repository.GetProductByCategoryAsync(category);
            return Ok(products);
        }

        [Authorize(Roles = "Admin, SuperAdmin")]
        [HttpPost]
        [ProducesResponseType(typeof(Product), StatusCodes.Status201Created)]
        public async Task<ActionResult<Product>> CreateProduct([FromBody] Product product)
        {
            product.Id = Guid.Empty;
            await _repository.CreateProductAsync(product);

            return StatusCode(StatusCodes.Status201Created, product);
        }

        [Authorize(Roles = "Admin, SuperAdmin")]
        [HttpPut]
        public async Task<ActionResult> UpdateProduct([FromBody] Product product)
        {
            var isSuccess = await _repository.UpdateProductAsync(product);

            if (!isSuccess)
                throw new Exception($"Updating product with id: {product.Id} failed.");

            return Ok(isSuccess);
        }

        [Authorize(Roles = "Admin, SuperAdmin")]
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> DeleteProductById(Guid id)
        {
            var isSuccess = await _repository.DeleteProductAsync(id);

            if (!isSuccess)
                throw new Exception($"deleting product with id: {id} failed.");

            return Ok(isSuccess);
        }
    }
}
