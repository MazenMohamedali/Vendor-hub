using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using VendorHub.Domain.Entities.Orders;

namespace VendorHub.Infrastructure.Persistence.Configurations
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.ToTable("OrderItems");

            builder.HasKey(i => i.Id);

            builder.OwnsOne(i => i.UnitPrice, price =>
            {
                price.Property(m => m.Amount)
                    .HasColumnName("UnitPrice")
                    .HasPrecision(18, 2)
                    .IsRequired();

                price.Property(m => m.Currency)
                    .HasColumnName("Currency")
                    .HasMaxLength(3)
                    .IsRequired();
            });

            builder.HasIndex(i => i.VendorId);
            builder.HasIndex(i => i.ProductId);
        }
    }
}
