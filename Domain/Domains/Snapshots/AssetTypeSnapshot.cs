using BaseDomain.Entity;

namespace Domain.Domains.Snapshots;

public class AssetTypeSnapshot : AuditedBaseEntity
{
    public string UserId { get; set; }
    public string AssetTypeId { get; set; }
    public double TotalInvested { get; set; }
    public double TotalAmount { get; set; }

    public AssetTypeSnapshot(string userId, string assetTypeId, double totalInvested, double totalAmount)
    {
        UserId = userId;
        AssetTypeId = assetTypeId;
        TotalInvested = totalInvested;
        TotalAmount = totalAmount;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    protected override void ValidateRules()
    {
        if (string.IsNullOrEmpty(UserId))
        {
            DomainValidation.AddNotification("UserId", "UserId is required");
        }

        if (string.IsNullOrEmpty(AssetTypeId))
        {
            DomainValidation.AddNotification("AssetTypeId", "AssetTypeId is required");
        }

        if (TotalInvested <= 0)
        {
            DomainValidation.AddNotification("TotalInvested", "Total Invested must be greater than 0");
        }

        if (TotalAmount <= 0)
        {
            DomainValidation.AddNotification("TotalAmount", "Total Amount must be greater than 0");
        }
    }
}
