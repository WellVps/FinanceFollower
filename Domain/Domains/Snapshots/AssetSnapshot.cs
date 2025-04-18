using BaseDomain.Entity;

namespace Domain.Domains.Snapshots;

public class AssetSnapshot: AuditedBaseEntity
{
    public string UserId { get; set; }
    public string AssetId { get; set; }
    public double LastPrice { get; set; }
    public double Amount { get; set; }
    public double TotalInvested { get; set; }
    public double TotalAmount { get; set; }

    public AssetSnapshot(string userId, string assetId, double lastPrice, double amount, double totalInvested, double totalAmount)
    {
        UserId = userId;
        AssetId = assetId;
        LastPrice = lastPrice;
        Amount = amount;
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

        if (string.IsNullOrEmpty(AssetId))
        {
            DomainValidation.AddNotification("AssetId", "AssetId is required");
        }

        if (LastPrice <= 0)
        {
            DomainValidation.AddNotification("LastPrice", "Last Price must be greater than 0");
        }

        if (Amount <= 0)
        {
            DomainValidation.AddNotification("Amount", "Amount must be greater than 0");
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
