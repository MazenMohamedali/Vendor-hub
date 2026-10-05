namespace VendorHub.Domain.Entities.Users;

using VendorHub.Domain.Common;
using VendorHub.Domain.Enums;
using VendorHub.Domain.Exceptions;

public abstract class User : AggregateRoot
{
    public string FirstName { get; protected set; } = string.Empty;
    public string LastName { get; protected set; } = string.Empty;
    public string Email { get; protected set; } = string.Empty;
    public string? PhoneNumber { get; protected set; }
    public AccountStatus Status { get; protected set; } = AccountStatus.Pending;

    protected User() { }

    public void UpdateProfile(string firstName, string lastName, string? phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new DomainException("First name is required.");
        if (string.IsNullOrWhiteSpace(lastName))
            throw new DomainException("Last name is required.");

        FirstName = firstName.Trim();
        LastName = lastName.Trim();
        PhoneNumber = phoneNumber?.Trim();
    }

    public void Activate() => Status = AccountStatus.Active;
    public void Suspend() => Status = AccountStatus.Suspended;
    public void Delete() => Status = AccountStatus.Deleted;
}
