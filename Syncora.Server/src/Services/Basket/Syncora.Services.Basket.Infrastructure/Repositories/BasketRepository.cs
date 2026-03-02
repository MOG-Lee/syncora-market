using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using Syncora.Services.Basket.Application.Interfaces;
using Syncora.Services.Basket.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Syncora.Services.Basket.Infrastructure.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly ILogger<BasketRepository> _logger;
        private readonly IDatabase _database;

        public BasketRepository(ILogger<BasketRepository> logger, IConnectionMultiplexer redis)
        {
            _logger = logger;
            _database = redis.GetDatabase();
        }

        public async Task<bool> DeleteBasketAsync(Guid buyerId)
        {
            return await _database.KeyDeleteAsync(buyerId.ToString());
        }

        public async Task<CustomerBasket?> GetBasketAsync(Guid buyerId)
        {
            var data = await _database.StringGetAsync(buyerId.ToString());

            if (data.IsNullOrEmpty)
                return null;

            return JsonSerializer.Deserialize<CustomerBasket>(data!, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }

        public async Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket basket)
        {
            var created = await _database.StringSetAsync(
                basket.BuyerId.ToString(),
                JsonSerializer.Serialize(basket),
                TimeSpan.FromDays(30)); // TTL: Automatically delete abandoned carts after 30 days!

            if (!created)
            {
                _logger.LogError("Problem occurring persisting the item.");
                return null;
            }

            return await GetBasketAsync(basket.BuyerId);
        }
    }
}
