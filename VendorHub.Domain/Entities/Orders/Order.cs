namespace VendorHub.Domain.Entities.Orders;

using System;
using System.Collections.Generic;
using System.Linq;
using VendorHub.Domain.Common;
using VendorHub.Domain.Enums;
using VendorHub.Domain.Events;
using VendorHub.Domain.Exceptions;
using VendorHub.Domain.ValueObjects;

public class Order : AggregateRoot
    {
        private readonly List<OrderItem> _items = new();
        public Guid CustomerId { get; private set; }
        public Address DeliveryAddress { get; private set; }
        public string PhoneNumber { get; private set; }
        public OrderStatus Status { get; private set; }
        public Money TotalPrice { get; private set; }
        public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();

        public int TotalItemsCount => _items.Sum(i => i.Quantity);
        public int ShippedItemsCount { get; private set; }
        public int DeliveredItemsCount { get; private set; }
        public int CancelledItemsCount { get; private set; }

        private Order() { }

        public static Order Create(Guid customerId, Address deliveryAddress, string phoneNumber, string currency = "EGP")
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                throw new DomainException("Phone number is required for delivery.");

            var order = new Order
            {
                CustomerId = customerId,
                DeliveryAddress = deliveryAddress,
                PhoneNumber = phoneNumber.Trim(),
                Status = OrderStatus.Pending,
                TotalPrice = Money.Zero(currency)
            };

            return order;
        }

        public void AddItem(Guid productId, Guid vendorId, Money unitPrice, int quantity)
        {
            if (Status != OrderStatus.Pending)
                throw new DomainException("Cannot add items to an order that is no longer pending.");                                                    


            var existingItem = _items.FirstOrDefault(i => i.ProductId == productId);
            if (existingItem != null)
                existingItem.AddQuantity(quantity);
            else
                _items.Add(new OrderItem(productId, vendorId, unitPrice, quantity));

            RecalculateTotal();
        }

        public void Cancel()
        {
            if (Status == OrderStatus.Delivered)
                throw new DomainException("Delivered orders cannot be cancelled.");

            if (Status == OrderStatus.Cancelled) return;

            Status = OrderStatus.Cancelled;
            AddDomainEvent(new OrderStatusChangedDomainEvent(this.Id, OrderStatus.Cancelled));
        }

        public void MarkItemShipped(Guid orderItemId)
        {
            var item = _items.FirstOrDefault(i => i.Id == orderItemId);
            if (item == null)
                throw new DomainException("Order item not found.");

            item.Ship();
            ShippedItemsCount++;

            if (_items.All(i => i.Status == OrderItemStatus.Shipped))
            {
                Status = OrderStatus.Shipped;
                AddDomainEvent(new OrderStatusChangedDomainEvent(this.Id, OrderStatus.Shipped));
            }
        }

        private void RecalculateTotal()
        {
            var currency = _items.FirstOrDefault()?.UnitPrice.Currency ?? "EGP";
            TotalPrice = _items.Aggregate(Money.Zero(currency), (sum, item) => sum + (item.UnitPrice * item.Quantity));
        }
    }
