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
    public class OrderEntityTypeConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(o => o.Id);
            builder.Property(o => o.Id).ValueGeneratedNever(); 

            builder.Property(o => o.BuyerId).IsRequired();
            builder.Property(o => o.OrderDate).IsRequired();
            builder.Property(o => o.Total).HasColumnType("decimal(18,2)"); // Money precision

            builder.OwnsOne(o => o.ShippingAddress, a =>
            {
                a.Property(x => x.Street).HasColumnName("ShippingStreet").HasMaxLength(100).IsRequired();
                a.Property(x => x.City).HasColumnName("ShippingCity").HasMaxLength(50).IsRequired();
                a.Property(x => x.Zipcode).HasColumnName("ShippingZipCode").HasMaxLength(20).IsRequired();
            });

            builder.Property(o => o.OrderStatus).HasConversion<string>().HasMaxLength(30);

            // 3. Private Collection Access
            // Tells EF to look for the private field "_orderItems" backing the property "OrderItems"
            builder.Metadata.FindNavigation(nameof(Order.OrderItems))!
                .SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
