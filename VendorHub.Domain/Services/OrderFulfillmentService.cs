namespace VendorHub.Domain.Services;

using System;
using System.Collections.Generic;
using System.Linq;
using VendorHub.Domain.Entities.Orders;
using VendorHub.Domain.Entities.Products;
using VendorHub.Domain.Exceptions;

public class OrderFulfillmentService
{
    public void FulfillOrder(Order order, IReadOnlyList<Product> products)
    {
        ArgumentNullException.ThrowIfNull(order);
        ArgumentNullException.ThrowIfNull(products);

        order.MarkAsProcessing();

        foreach (var item in order.Items)
        {
            var product = products.FirstOrDefault(p => p.Id == item.ProductId);
            if (product is null)
            {
                throw new DomainException($"Product with ID '{item.ProductId}' was not found.");
            }

            // Deducts stock and triggers ProductStockDepletedDomainEvent if depleted
            product.DeductStock(item.Quantity);
        }

        order.CompleteProcessing();
    }
}
