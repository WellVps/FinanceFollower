using BaseDomain.Entity;

namespace Domain.Domains.Assets;

public class AssetType: AuditedBaseEntity
{
    public string Description { get; set; }
    public bool Active { get; set; }
    public string? Icon { get; set; }

    public AssetType(string description, bool active = true, string? icon = null)
    {
        Description = description;
        Active = active;
        Icon = icon;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    protected override void ValidateRules()
    {
        if (string.IsNullOrEmpty(Description))
        {
            DomainValidation.AddNotification("Description", "Description is required");
        }
    }
}