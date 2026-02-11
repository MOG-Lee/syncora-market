using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Syncora.Services.Ordering.Domain.AggregatesModel.OrderAggregate;
using Syncora.SharedKernel.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Syncora.Services.Ordering.Infrastructure.Persistence
{
    //Add-Migration InitialCreate -Context OrderingDbContext
    //Update-Database -Context OrderingDbContext

    public class OrderingDbContext : DbContext, IUnitOfWork
    {
        private readonly IMediator _mediator;

        // We inject Mediator to dispatch Domain Events
        public OrderingDbContext(DbContextOptions<OrderingDbContext> options, IMediator mediator) : base(options)
        {
            _mediator = mediator;
        }

        public DbSet<Order> Orders { get; set; }

        // Only Aggregate Roots should be Dbsets
        //public DbSet<OrderItem> OrderItems { get; set; }

        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
        {
            // 1. CRITICAL: Actually dispatch the events before saving!
            // This Extension Method (we need to create it) moves events from Entities -> MediatR
            //await _mediator.DispatchDomainEventsAsync(this);

            // 2. Commit the transaction (Saves Order + Outbox Messages)
            await base.SaveChangesAsync(cancellationToken);
            return true;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Ignore<DomainEvent>(); 

            // Scan files implementing IEntityTypeConfiguration
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            // 3. CRITICAL: Add MassTransit Outbox Tables
            // Without this, MassTransit can't save the messages to the DB
            //modelBuilder.AddInboxStateEntity();
            //modelBuilder.AddOutboxMessageEntity();
            //modelBuilder.AddOutboxStateEntity();

            base.OnModelCreating(modelBuilder);
        }
    }
}
