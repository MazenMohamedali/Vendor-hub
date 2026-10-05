using System;
using System.Collections.Generic;
using System.Text;
using VendorHub.Domain.Exceptions;

namespace VendorHub.Domain.ValueObjects;

public sealed record Money
    {
        public decimal Amount { get; init; }
        public string Currency { get; init; }

        // Required for EF Core materialization
        private Money() { }

        public Money(decimal amount, string currency = "EGP")
        {
            if (amount < 0)
                throw new DomainException("Money amount cannot be negative.");

            if (string.IsNullOrWhiteSpace(currency))
                throw new DomainException("Currency code is required.");

            Amount = decimal.Round(amount, 2, MidpointRounding.AwayFromZero);
            Currency = currency.Trim().ToUpperInvariant();
        }

        public static Money Zero(string currency = "EGP") => new(0m, currency);

        public Money Add(Money other)
        {
            EnsureSameCurrency(other);
            return new Money(Amount + other.Amount, Currency);
        }

        public Money Subtract(Money other)
        {
            EnsureSameCurrency(other);
            if (Amount - other.Amount < 0)
                throw new DomainException("Resulting amount cannot be negative.");                                                                


            return new Money(Amount - other.Amount, Currency);
        }

        public Money Multiply(int quantity)
        {
            if (quantity < 0)
                throw new DomainException("Multiplier quantity cannot be negative.");                                                                


            return new Money(Amount * quantity, Currency);
        }

        private void EnsureSameCurrency(Money other)
        {
            if (Currency != other.Currency)
                throw new DomainException($"Currency mismatch: cannot operate between '{Currency}' and '{other.Currency}'.");                             
        }

        // Operator Overloads                                                 
        public static Money operator +(Money left, Money right) => left.Add(right);
        public static Money operator -(Money left, Money right) => left.Subtract(right);
        public static Money operator *(Money left, int quantity) => left.Multiply(quantity);

        public override string ToString() => $"{Amount:N2} {Currency}";
}
