using BaseDomain.Entity;

namespace Domain.Domains.Investments;

public class Investments: AuditedBaseEntity
{
    public DateTime LastBought { get; set; }
    public double Quantity { get; set; }
    public double AveragePrice { get; set; }
    public double TotalInvested { get; set; }
    public double Price { get; set; }
    public double TotalAmount { get; set; }

    public Investments(DateTime lastBought, double quantity, double averagePrice, double totalInvested, double price, double totalAmount)
    {
        LastBought = lastBought;
        Quantity = quantity;
        AveragePrice = averagePrice;
        TotalInvested = totalInvested;
        Price = price;
        TotalAmount = totalAmount;
        CreatedAt = DateTime.UtcNow;
        Validate();
    }

    protected override void ValidateRules()
    {
        if (LastBought == DateTime.MinValue)
        {
            DomainValidation.AddNotification("LastBought", "Last Bought is required");
        }

        if (Quantity <= 0)
        {
            DomainValidation.AddNotification("Quantity", "Quantity must be greater than 0");
        }

        if (AveragePrice <= 0)
        {
            DomainValidation.AddNotification("AveragePrice", "Average Price must be greater than 0");
        }

        if (TotalInvested <= 0)
        {
            DomainValidation.AddNotification("TotalInvested", "Total Invested must be greater than 0");
        }

        if (Price <= 0)
        {
            DomainValidation.AddNotification("Price", "Price must be greater than 0");
        }

        if (TotalAmount <= 0)
        {
            DomainValidation.AddNotification("TotalAmount", "Total Amount must be greater than 0");
        }
    }
}