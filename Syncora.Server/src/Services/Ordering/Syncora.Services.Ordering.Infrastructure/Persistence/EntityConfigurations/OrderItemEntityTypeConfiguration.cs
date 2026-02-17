using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Syncora.Services.Ordering.Domain.AggregatesModel.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncora.Services.Ordering.Infrastructure.Persistence.EntityConfigurations
{
    public class OrderItemEntityTypeConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.ToTable("OrderItems");

            builder.HasKey(ci => ci.Id);
            builder.Property(ci => ci.Id).ValueGeneratedNever();

            builder.Property(ci => ci.ProductName).IsRequired().HasMaxLength(100);
            builder.Property(ci => ci.PictureUrl).IsRequired(false);
            builder.Property(ci => ci.UnitPrice).HasColumnType("decimal(18,2)");

            builder.Property(ci => ci.Units).IsRequired();
            builder.Property(ci => ci.ProductId).IsRequired();
        }
    }
}
