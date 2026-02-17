using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Syncora.Services.Inventory.Application.Interfaces;
using Syncora.Services.Inventory.Application.InventoryItems.Queries.GetInventoryStock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncora.Services.Inventory.Infrastructure.Services
{
    public class InventoryQueryService : IInventoryQueryService
    {
        private readonly string _connectionString;
        public InventoryQueryService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("InventoryDb")
                ?? throw new ArgumentNullException("Connection string 'InventoryDb' not found.");
        }

        public async Task<InventoryStockDto> GetInventoryStockAsync(Guid productId)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var sql = @"SELECT AvailableStock FROM InventoryItems 
                            WHERE ProductId = @productId";

            var stock = await connection.QueryFirstOrDefaultAsync<int?>(sql, new { productId });

            return new InventoryStockDto
            {
                AvailableStock = stock ?? 0
            };
        }
    }
}
