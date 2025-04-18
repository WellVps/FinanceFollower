using System;
using BaseDomain.Entity;

namespace Domain.Domains.Snapshots;

public class UserSnapshot: AuditedBaseEntity
{
    public string UserId { get; set; }
    public double TotalInvested { get; set; }
    public double TotalAmount { get; set; }

    public UserSnapshot(string userId, double totalInvested, double totalAmount)
    {
        UserId = userId;
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
