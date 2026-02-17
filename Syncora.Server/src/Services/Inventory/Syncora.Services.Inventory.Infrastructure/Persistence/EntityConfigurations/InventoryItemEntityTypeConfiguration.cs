using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Syncora.Services.Inventory.Domain.AggregatesModel.InventoryAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncora.Services.Inventory.Infrastructure.Persistence.EntityConfigurations
{
    public class InventoryItemEntityTypeConfiguration : IEntityTypeConfiguration<InventoryItem>
    {
        public void Configure(EntityTypeBuilder<InventoryItem> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedNever();

            builder.Property(x => x.ProductId).IsRequired();
            builder.Property(x => x.AvailableStock).IsRequired();

            // Concurrency Token
            // If 2 orders try to buy the last item at the exact same millisecond, 
            // one will fail with DbUpdateConcurrencyException.
            builder.Property(x => x.RowVersion).IsRowVersion();
        }
    }
}
