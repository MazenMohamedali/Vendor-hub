namespace VendorHub.Domain.Common;

public class AuditableEntity : BaseEntity
{
    public DateTime CreatedAtUtc { get; protected set; }
    public string? CreatedBy { get; protected set; }

    public DateTime? LastModifiedAtUtc { get; protected set; }
    public string? LastModifiedBy { get; protected set; }

    protected AuditableEntity()
    {
        CreatedAtUtc = DateTime.UtcNow;
    }

    public void SetCreated(string? createdBy, DateTime? createdAtUtc = null)
    {
        CreatedBy = createdBy;
        CreatedAtUtc = createdAtUtc ?? DateTime.UtcNow;
    }

    public void SetModified(string? modifiedBy, DateTime? modifiedAtUtc = null)
    {
        LastModifiedBy = modifiedBy;
        LastModifiedAtUtc = modifiedAtUtc ?? DateTime.UtcNow;
    }
}
