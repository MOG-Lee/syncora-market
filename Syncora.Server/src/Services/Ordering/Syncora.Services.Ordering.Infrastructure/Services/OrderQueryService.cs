using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Syncora.Services.Ordering.Application.Interfaces;
using Syncora.Services.Ordering.Application.Orders.Queries.GetOrderDetails;
using Syncora.Services.Ordering.Application.Orders.Queries.GetOrderSummary;
using Syncora.SharedKernel.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncora.Services.Ordering.Infrastructure.Services
{
    public class OrderQueryService : IOrderQueryService
    {
        private readonly string _connectionString;

        public OrderQueryService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("OrderingDb")
             ?? throw new DomainException("Connection string 'OrderingDb' not found.");
        }

        public async Task<OrderDetailDto> GetOrderAsync(Guid id)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            var sqlOrder = @"
            SELECT o.Id as OrderId, o.OrderDate as Date, o.OrderStatus as Status, o.Total,
                   o.ShippingStreet as Street, o.ShippingCity as City, o.ShippingZipCode as ZipCode
            FROM Orders o
            WHERE o.Id = @id";

            var order = await connection.QueryFirstOrDefaultAsync<OrderDetailDto>(sqlOrder, new { id });

            if (order == null)
                throw new KeyNotFoundException($"Order {id} not found");  

            var sqlItems = @"
                SELECT ProductName, PictureUrl, Units, UnitPrice
                FROM OrderItem
                WHERE OrderId = @id"; 

            var items = await connection.QueryAsync<OrderViewItemDto>(sqlItems, new { id });

            if(items == null || !items.Any())
                throw new KeyNotFoundException($"No items found for order {id}");

            return order with { OrderItems = items };
        }

        public async Task<IEnumerable<OrderSummaryDto>> GetOrdersFromUserAsync(Guid userId)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open(); 

            var sql = @"
            SELECT Id as OrderId, OrderDate as Date, OrderStatus as Status, Total
            FROM Orders
            WHERE BuyerId = @userId
            ORDER BY OrderDate DESC";

            return await connection.QueryAsync<OrderSummaryDto>(sql, new { userId });
        }
    }
}
