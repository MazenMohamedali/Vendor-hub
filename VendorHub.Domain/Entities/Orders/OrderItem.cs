namespace VendorHub.Domain.Entities.Orders;

using System;
using System.Collections.Generic;
using VendorHub.Domain.Common;
using VendorHub.Domain.Enums;
using VendorHub.Domain.Exceptions;
using VendorHub.Domain.ValueObjects;

public class OrderItem : BaseEntity
    {
        public Guid ProductId { get; private set; }
        public Guid VendorId { get; private set; }
        public Money UnitPrice { get; private set; }
        public int Quantity { get; private set; }
        public OrderItemStatus Status { get; private set; }

        private OrderItem() { }

        internal OrderItem(Guid productId, Guid vendorId, Money unitPrice, int quantity)
        {
            if (quantity <= 0)
                throw new DomainException("Item quantity must be greater than zero.");

            ProductId = productId;
            VendorId = vendorId;
            UnitPrice = unitPrice;
            Quantity = quantity;
            Status = OrderItemStatus.Pending;
        }

        internal void AddQuantity(int additionalQuantity)
        {
            if (additionalQuantity <= 0)
                throw new DomainException("Additional quantity must be greater than zero.");                                                               

            Quantity += additionalQuantity;
        }

        internal void Ship()
        {
            if (Status != OrderItemStatus.Pending)
                throw new DomainException($"Cannot ship item with status {Status}.");                                                                

            Status = OrderItemStatus.Shipped;
        }
    }
