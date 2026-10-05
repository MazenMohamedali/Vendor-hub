namespace VendorHub.Domain.Entities.Products;

using System;
using System.Collections.Generic;
using VendorHub.Domain.Common;
using VendorHub.Domain.Exceptions;

public class Category : AggregateRoot
{
    public string Name { get; private set; }
    public string? ImageUrl { get; private set; }
    public bool IsActive { get; private set; }

    private readonly List<Product> _products = new();
    public IReadOnlyCollection<Product> Products => _products.AsReadOnly();

    private Category() { }

    public static Category Create(string name, string? imageUrl = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Category name is required.");

        return new Category
        {
            Name = name.Trim(),
            ImageUrl = imageUrl?.Trim(),
            IsActive = true
        };
    }

    public void Update(string name, string? imageUrl)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Category name is required.");

        Name = name.Trim();
        ImageUrl = imageUrl?.Trim();
    }

    public void Activate() => IsActive = true;
    public void Deactivate() => IsActive = false;
}
