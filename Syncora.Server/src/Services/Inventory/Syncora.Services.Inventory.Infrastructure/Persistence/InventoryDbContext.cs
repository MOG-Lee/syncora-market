using MassTransit;
using Microsoft.EntityFrameworkCore;
using Syncora.Services.Inventory.Domain.AggregatesModel.InventoryAggregate;
using Syncora.SharedKernel.Domain;
using Syncora.SharedKernel.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Syncora.Services.Inventory.Infrastructure.Persistence
{
    //Add-Migration InitialCreate -Context InventoryDbContext
    //Update-Database -Context InventoryDbContext
    public class InventoryDbContext : DbContext, IUnitOfWork
    {
        public InventoryDbContext(DbContextOptions<InventoryDbContext> options) : base(options)
        {

        }

        public DbSet<InventoryItem> InventoryItems { get; set; }

        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                await base.SaveChangesAsync(cancellationToken);
                return true;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new CustomConcurrencyException("Data was modified by another process. Please try again.", ex);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Ignore<DomainEvent>();

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            //Enable MassTransit Outbox Tables
            modelBuilder.AddInboxStateEntity();
            modelBuilder.AddOutboxMessageEntity();
            modelBuilder.AddOutboxStateEntity();

            base.OnModelCreating(modelBuilder);
        }
    }
}
