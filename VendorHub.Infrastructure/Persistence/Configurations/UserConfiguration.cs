namespace VendorHub.Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VendorHub.Domain.Entities.Users;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("DomainUsers");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.FirstName).HasMaxLength(100).IsRequired();
        builder.Property(u => u.LastName).HasMaxLength(100).IsRequired();
        builder.Property(u => u.Email).HasMaxLength(256).IsRequired();
        builder.Property(u => u.PhoneNumber).HasMaxLength(20);

        builder.HasIndex(u => u.Email).IsUnique();

        builder.HasDiscriminator<string>("UserType")
            .HasValue<Vendor>("Vendor")
            .HasValue<Customer>("Customer");
    }
}

public class VendorConfiguration : IEntityTypeConfiguration<Vendor>
{
    public void Configure(EntityTypeBuilder<Vendor> builder)
    {
        builder.Property(v => v.StoreName).HasMaxLength(150);

        // Value Object: Money Balance
        builder.OwnsOne(v => v.Balance, price =>
        {
            price.Property(m => m.Amount).HasColumnName("Balance").HasPrecision(18, 2);
            price.Property(m => m.Currency).HasColumnName("BalanceCurrency").HasMaxLength(3);
        });
    }
}

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        // Value Object: Address
        builder.OwnsOne(c => c.ShippingAddress, address =>
        {
            address.Property(a => a.Street).HasColumnName("ShippingStreet").HasMaxLength(200);
            address.Property(a => a.City).HasColumnName("ShippingCity").HasMaxLength(100);
            address.Property(a => a.State).HasColumnName("ShippingState").HasMaxLength(100);
            address.Property(a => a.Country).HasColumnName("ShippingCountry").HasMaxLength(50);
            address.Property(a => a.PostalCode).HasColumnName("ShippingPostalCode").HasMaxLength(20);
        });
    }
}
