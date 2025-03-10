using BaseDomain.Entity;

namespace Domain.Domains.Investments;

public class InvestmentEntry: AuditedBaseEntity
{
    public DateTime InvestmentDate { get; set; }
    public double TotalAmount { get; set; }
    public double TotalTax { get; set; }
    public List<AssetsList> Assets { get; set; }

    public class AssetsList
    {
        public string IdAsset { get; set; }
        public double Price { get; set; }
        public double Quantity { get; set; } 
        public double Amount { get; set; }
        public double Tax { get; set; }
    }

    protected override void ValidateRules()
    {
        if (InvestmentDate == DateTime.MinValue)
        {
            DomainValidation.AddNotification("InvestmentDate", "Investment Date is required");
        }

        if (TotalAmount <= 0)
        {
            DomainValidation.AddNotification("Amount", "Amount must be greater than 0");
        }

        if (Assets == null || !Assets.Any())
        {
            DomainValidation.AddNotification("Assets", "At least one asset is required");
        }
        else
        {
            foreach (var asset in Assets)
            {
                if (string.IsNullOrEmpty(asset.IdAsset))
                {
                    DomainValidation.AddNotification("IdAsset", "Asset is required");
                }

                if (asset.Price <= 0)
                {
                    DomainValidation.AddNotification("Price", "Price must be greater than 0");
                }

                if (asset.Quantity <= 0)
                {
                    DomainValidation.AddNotification("Quantity", "Quantity must be greater than 0");
                }

                if (asset.Amount <= 0)
                {
                    DomainValidation.AddNotification("Amount", "Amount must be greater than 0");
                }
            }
        }
    }
}