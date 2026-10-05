using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VendorHub.Domain.Entities.Orders;

namespace VendorHub.Infrastructure.Persistence.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders");

            builder.HasKey(o => o.Id);

            builder.Property(o => o.PhoneNumber)
                .HasMaxLength(20)
                .IsRequired();

            builder.OwnsOne(o => o.DeliveryAddress, address =>
            {
                address.Property(a => a.Street).HasColumnName("Street").HasMaxLength(200).IsRequired();
                address.Property(a => a.City).HasColumnName("City").HasMaxLength(100).IsRequired();
                address.Property(a => a.State).HasColumnName("State").HasMaxLength(100);
                address.Property(a => a.Country).HasColumnName("Country").HasMaxLength(50).IsRequired();
                address.Property(a => a.PostalCode).HasColumnName("PostalCode").HasMaxLength(20);
            });

            builder.OwnsOne(o => o.TotalPrice, price =>
            {
                price.Property(m => m.Amount)
                    .HasColumnName("TotalPrice")
                    .HasPrecision(18, 2)
                    .IsRequired();

                price.Property(m => m.Currency)
                    .HasColumnName("Currency")
                    .HasMaxLength(3)
                    .IsRequired();
            });

            builder.HasMany(o => o.Items)
                .WithOne()
                .HasForeignKey("OrderId")
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.Navigation(o => o.Items)
                .UsePropertyAccessMode(PropertyAccessMode.Field);

            builder.HasIndex(o => o.CustomerId);
        }
    }
}
