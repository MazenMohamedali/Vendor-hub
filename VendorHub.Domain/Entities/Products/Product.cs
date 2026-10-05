namespace VendorHub.Domain.Entities.Products;

using System;
using System.Collections.Generic;
using VendorHub.Domain.Common;
using VendorHub.Domain.Enums;
using VendorHub.Domain.Events;
using VendorHub.Domain.Exceptions;
using VendorHub.Domain.ValueObjects;

public class Product : AggregateRoot
    {
        public string Name { get; private set; }
        public Money Price { get; private set; }
        public string ImgUrl { get; private set; }
        public int Quantity { get; private set; }
        public ProductStatus Status { get; private set; }
        public Guid VendorId { get; private set; }
        public Guid CategoryId { get; private set; }
        public long ViewersCount { get; private set; }

        private Product() { }

        public static Product Create(string name, Money price, string imgUrl, int quantity, Guid vendorId, Guid categoryId)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("Product name is required.");

            if (quantity < 0)
                throw new DomainException("Initial quantity cannot be negative.");

            var product = new Product
            {
                Name = name.Trim(),
                Price = price,
                ImgUrl = imgUrl?.Trim() ?? string.Empty,
                Quantity = quantity,
                VendorId = vendorId,
                CategoryId = categoryId,
                Status = ProductStatus.Pending,
                ViewersCount = 0
            };

            product.AddDomainEvent(new ProductCreatedDomainEvent(product.Id, name, vendorId));
            return product;
        }

        public void DeductStock(int amount)
        {
            if (amount <= 0)
                throw new DomainException("Deduction amount must be greater than zero.");                                                               

            if (Quantity < amount)
                throw new InsufficientStockException(Name, Quantity, amount);

            Quantity -= amount;

            if (Quantity == 0)
            {
                AddDomainEvent(new ProductStockDepletedDomainEvent(this.Id, this.Name, this.VendorId));
            }
        }

        public void Restock(int amount)
        {
            if (amount <= 0)
                throw new DomainException("Restock amount must be greater than zero.");                                                                    

            Quantity += amount;
        }

        public void Approve()
        {
            if (Status == ProductStatus.Reviewed)
                return;

            Status = ProductStatus.Reviewed;
            AddDomainEvent(new ProductApprovedDomainEvent(this.Id, this.VendorId));
        }

        public void Reject(string reason)
        {
            if (string.IsNullOrWhiteSpace(reason))
                throw new DomainException("A rejection reason must be provided.");                                                                

            Status = ProductStatus.Rejected;
            AddDomainEvent(new ProductRejectedDomainEvent(this.Id, this.VendorId, reason));
        }

        public void IncrementViewers()
        {
            ViewersCount++;
        }
    }
