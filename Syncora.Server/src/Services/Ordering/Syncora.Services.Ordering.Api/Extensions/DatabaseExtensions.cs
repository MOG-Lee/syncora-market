using Microsoft.EntityFrameworkCore;
using Syncora.Services.Ordering.Infrastructure.Persistence;

namespace Syncora.Services.Ordering.Api.Extensions
{
    public static class DatabaseExtensions
    {
        public static async Task ApplyDatabaseMigrationAsync(this WebApplication app)
        {
            // create a scope like a fake HTTP request
            using var scope = app.Services.CreateScope();
            //get service provider 
            var context = scope.ServiceProvider.GetRequiredService<OrderingDbContext>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<OrderingDbContext>>();

            try
            {
                logger.LogInformation("Migrating database associated with context {DbContextName}", typeof(OrderingDbContext).Name);

                await context.Database.MigrateAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while migrating the database.");
                throw;
            }
        }
    }
}
