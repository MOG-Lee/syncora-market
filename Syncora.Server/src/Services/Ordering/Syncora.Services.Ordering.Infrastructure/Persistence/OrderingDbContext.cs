using MassTransit;
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
            // This runs the "Bridge Handler" -> Publishes to Outbox -> Adds to ChangeTracker
            await _mediator.DispatchDomainEventsAsync(this);

            // Commit EVERYTHING (Order + Outbox Messages) in one SQL Transaction
            await base.SaveChangesAsync(cancellationToken);
            return true;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Ignore<DomainEvent>();

            // Scan files implementing IEntityTypeConfiguration
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            //Enable MassTransit Outbox Tables
            modelBuilder.AddInboxStateEntity();
            modelBuilder.AddOutboxMessageEntity();
            modelBuilder.AddOutboxStateEntity();


            base.OnModelCreating(modelBuilder);
        }
    }
}
