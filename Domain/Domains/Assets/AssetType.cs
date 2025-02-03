using BaseDomain;

namespace Domain.Domains.Assets;

public class AssetType: BaseEntity
{
    public string Description { get; set; }
    public bool Active { get; set; }
    public string? Icon { get; set; }

    public AssetType(string description, bool active = true, string? icon = null)
    {
        Description = description;
        Active = active;
        Icon = icon;
    }

    protected override void ValidateRules()
    {
        if (string.IsNullOrEmpty(Description))
        {
            DomainValidation.AddNotification("Description", "Description is required");
        }
    }
}