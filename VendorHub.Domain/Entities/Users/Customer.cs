namespace VendorHub.Domain.Entities.Users;

using VendorHub.Domain.Enums;
using VendorHub.Domain.Exceptions;
using VendorHub.Domain.ValueObjects;

public class Customer : User
{
    public Address? ShippingAddress { get; private set; }

    private Customer() { }

    public static Customer Create(string firstName, string lastName, string email, Address? shippingAddress = null)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new DomainException("Email is required.");

        var customer = new Customer
        {
            ShippingAddress = shippingAddress,
            Status = AccountStatus.Active
        };

        customer.UpdateProfile(firstName, lastName, null);
        customer.Email = email.Trim().ToLowerInvariant();
        return customer;
    }

    public void UpdateShippingAddress(Address address)
    {
        ShippingAddress = address ?? throw new DomainException("Address cannot be null.");
    }
}
