using BaseDomain.Entity;

namespace Domain.Domains.Assets;

public class Asset : AuditedBaseEntity
{
    public string Ticker { get; set; }
    public string Name { get; set; }
    public string IdAssetType { get; set; }
    public string DataSource { get; set; }
    public double LastPrice { get; set; }
    public bool Active { get; set; } = true;

    public Asset(string ticker, string name, string idAssetType, string dataSource)
    {
        Ticker = ticker;
        Name = name;
        IdAssetType = idAssetType;
        CreatedAt = DateTime.UtcNow;
        DataSource = dataSource;
        Validate();
    }

    protected override void ValidateRules()
    {
        if (string.IsNullOrEmpty(Ticker))
        {
            DomainValidation.AddNotification("Ticker", "Ticker is required");
        }

        if (string.IsNullOrEmpty(Name))
        {
            DomainValidation.AddNotification("Name", "Name is required");
        }

        if (string.IsNullOrEmpty(IdAssetType))
        {
            DomainValidation.AddNotification("IdAssetType", "Asset Type is required");
        }
    }
}