namespace VendorHub.Domain.Entities.Users;

using VendorHub.Domain.Enums;
using VendorHub.Domain.Exceptions;
using VendorHub.Domain.ValueObjects;

public class Vendor : User
{
    public string StoreName { get; private set; } = string.Empty;
    public Money Balance { get; private set; }
    public PermissionType Permissions { get; private set; }

    private Vendor() { }

    public static Vendor Create(string firstName, string lastName, string email, string storeName, string currency = "EGP")
    {
        if (string.IsNullOrWhiteSpace(storeName))
            throw new DomainException("Store name is required.");
        if (string.IsNullOrWhiteSpace(email))
            throw new DomainException("Email is required.");

        var vendor = new Vendor
        {
            StoreName = storeName.Trim(),
            Balance = Money.Zero(currency),
            Permissions = PermissionType.VendorAdmin,
            Status = AccountStatus.Active
        };

        vendor.UpdateProfile(firstName, lastName, null);
        vendor.Email = email.Trim().ToLowerInvariant();
        return vendor;
    }

    public void UpdateStoreName(string storeName)
    {
        if (string.IsNullOrWhiteSpace(storeName))
            throw new DomainException("Store name cannot be empty.");
        StoreName = storeName.Trim();
    }

    public void CreditBalance(Money amount)
    {
        Balance = Balance.Add(amount);
    }

    public void DebitBalance(Money amount)
    {
        Balance = Balance.Subtract(amount);
    }

    public void UpdatePermissions(PermissionType permissions)
    {
        Permissions = permissions;
    }
}
