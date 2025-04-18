using Domain.Enums;
using BaseDomain.Entity;

namespace Domain.Domains.Assets;

public class Asset : AuditedBaseEntity
{
    public string Ticker { get; set; }
    public string InternalTicker { get; set; }
    public string Name { get; set; }
    public string AssetTypeId { get; set; }
    public double LastPrice { get; set; }
    public Integrator Integrator { get; set; }
    public bool Active { get; set; } = true;

    public Asset(string ticker, string name, string idAssetType, Integrator integrator, string? internalTicker = null)
    {
        Ticker = ticker;
        Name = name;
        AssetTypeId = idAssetType;
        CreatedAt = DateTime.UtcNow;
        Integrator = integrator;
        InternalTicker = string.IsNullOrWhiteSpace(internalTicker) ? ticker : internalTicker;
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

        if (string.IsNullOrEmpty(AssetTypeId))
        {
            DomainValidation.AddNotification("IdAssetType", "Asset Type is required");
        }
    }
}