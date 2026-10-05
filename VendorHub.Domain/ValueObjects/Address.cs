using System;
using System.Collections.Generic;
using System.Text;
using VendorHub.Domain.Exceptions;

namespace VendorHub.Domain.ValueObjects;

public sealed record Address
{
    public string Street { get; init; }
    public string City { get; init; }
    public string State { get; init; }
    public string Country { get; init; }
    public string PostalCode { get; init; }

    // Required for EF Core materialization                               
    private Address() { }

    public Address(string street, string city, string state, string country, string postalCode)
    {
        if (string.IsNullOrWhiteSpace(street))
            throw new DomainException("Street address is required.");

        if (string.IsNullOrWhiteSpace(city))
            throw new DomainException("City is required.");

        if (string.IsNullOrWhiteSpace(country))
            throw new DomainException("Country is required.");

        Street = street.Trim();
        City = city.Trim();
        State = state?.Trim() ?? string.Empty;
        Country = country.Trim().ToUpperInvariant();
        PostalCode = postalCode?.Trim() ?? string.Empty;
    }

    public override string ToString() =>
        string.IsNullOrWhiteSpace(State)
            ? $"{Street}, {City}, {Country} {PostalCode}"
            : $"{Street}, {City}, {State}, {Country} {PostalCode}";
}
