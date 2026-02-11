using Syncora.Services.Ordering.Application.Interfaces;
using Syncora.Services.Ordering.Application.Orders.Queries.GetOrderDetails;
using Syncora.Services.Ordering.Application.Orders.Queries.GetOrderSummary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncora.Services.Ordering.Application.Interfaces
{
    public interface IOrderQueryService
    {
        Task<OrderDetailDto> GetOrderAsync(Guid id);
        Task<IEnumerable<OrderSummaryDto>> GetOrdersFromUserAsync(Guid userId);
    }
}


// ******** WE use Dapper (micro ORM) and Microsoft.Data.SqlClient (we should install both)
// for getting not EF which implement advanced options

//public class OrderQueryService : IOrderQueryService
//{
//    // 1. We need the connection string to talk to the DB
//    private readonly string _connectionString;

//    public OrderQueryService(IConfiguration configuration)
//    {
//        // Assuming your appsettings.json has a "ConnectionStrings:DefaultConnection" section
//        _connectionString = configuration.GetConnectionString("DefaultConnection")
//                            ?? throw new ArgumentNullException("Connection string not found");
//    }

//    // METHOD 1: Simple List (Get Orders for User)
//    public async Task<IEnumerable<OrderSummaryDto>> GetOrdersFromUserAsync(Guid userId)
//    {
//        using var connection = new SqlConnection(_connectionString);
//        connection.Open();

//        // 2. We write RAW SQL. 
//        // Dapper matches the column names (e.g., "OrderDate") to the property names in OrderSummaryDto.
//        var sql = @"
//                SELECT 
//                    o.Id as OrderId, 
//                    o.OrderDate as Date, 
//                    o.OrderStatus as Status, 
//                    o.Total,
//                    (SELECT COUNT(*) FROM OrderItems WHERE OrderId = o.Id) as TotalItems
//                FROM Orders o
//                WHERE o.BuyerId = @userId
//                ORDER BY o.OrderDate DESC";

//        // 3. Dapper Magic: connection.QueryAsync<Type>
//        // passing 'new { userId }' automatically prevents SQL Injection
//        return await connection.QueryAsync<OrderSummaryDto>(sql, new { userId });
//    }

//    // METHOD 2: Complex Object (Header + Details List)
//    public async Task<OrderDetailDto> GetOrderAsync(Guid id)
//    {
//        using var connection = new SqlConnection(_connectionString);
//        connection.Open();

//        // 4. We write TWO queries separated by a semicolon.
//        // Query 1: The Order Header (Flattened Address)
//        // Query 2: The List of Items
//        var sql = @"
//                SELECT 
//                    o.Id as OrderId, 
//                    o.OrderDate as Date, 
//                    o.OrderStatus as Status, 
//                    o.Total,
//                    o.Address_Street as Street, 
//                    o.Address_City as City, 
//                    o.Address_ZipCode as ZipCode, 
//                    o.Address_Country as Country
//                FROM Orders o
//                WHERE o.Id = @id;

//                SELECT 
//                    oi.ProductName, 
//                    oi.PictureUrl, 
//                    oi.Units, 
//                    oi.UnitPrice
//                FROM OrderItems oi
//                WHERE oi.OrderId = @id;";

//        // 5. use QueryMultipleAsync to fetch multiple result sets in one go
//        using var multi = await connection.QueryMultipleAsync(sql, new { id });

//        // Read the first result (Order Header)
//        // SingleOrDefaultAsync returns null if not found
//        var orderHeader = await multi.ReadSingleOrDefaultAsync<dynamic>();

//        if (orderHeader == null)
//            throw new KeyNotFoundException($"Order with id {id} not found.");

//        // Read the second result (Items)
//        var items = await multi.ReadAsync<Syncora.Services.Ordering.Application.Orders.Queries.ViewModels.OrderItemDto>();

//        // 6. Assemble the final DTO manually
//        // Since Dapper returned a 'dynamic' for header, we cast properties explicitly
//        // (Or you could create a private class for the header mapping if you prefer strong typing)
//        return new OrderDetailDto(
//            orderHeader.OrderId,
//            orderHeader.Date,
//            ((int)orderHeader.Status).ToString(), // Converting Enum ID to String/Text if needed
//            orderHeader.Street,
//            orderHeader.City,
//            orderHeader.ZipCode,
//            orderHeader.Country,
//            orderHeader.Total,
//            items.ToList() // Attach the items list
//        );
//    }
//}