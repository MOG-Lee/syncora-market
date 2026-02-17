using Microsoft.EntityFrameworkCore;
using Syncora.Services.Inventory.Infrastructure.Persistence;

namespace Syncora.Services.Inventory.Api.Extensions
{
    public static class DatabaseExtensions
    {
        public static async Task ApplyDatabaseMigrationAsync(this WebApplication app)
        {
            // create a scope like a fake HTTP request
            using var scope = app.Services.CreateScope();
            //get service provider 
            var context = scope.ServiceProvider.GetRequiredService<InventoryDbContext>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<InventoryDbContext>>();

            try
            {
                logger.LogInformation("Migrating database associated with context {DbContextName}", typeof(InventoryDbContext).Name);

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
