namespace VendorHub.Domain.Entities.Reviews;

using System;
using VendorHub.Domain.Common;
using VendorHub.Domain.Exceptions;

public class Review : BaseEntity
{
    public int Rating { get; private set; }
    public string? Comment { get; private set; }
    public Guid CustomerId { get; private set; }
    public Guid ProductId { get; private set; }
    public DateTime CreatedAtUtc { get; private set; } = DateTime.UtcNow;

    private Review() { }

    public static Review Create(Guid customerId, Guid productId, int rating, string? comment = null)
    {
        if (rating < 1 || rating > 5)
            throw new DomainException("Rating must be between 1 and 5 stars.");

        return new Review
        {
            CustomerId = customerId,
            ProductId = productId,
            Rating = rating,
            Comment = comment?.Trim(),
            CreatedAtUtc = DateTime.UtcNow
        };
    }
}
