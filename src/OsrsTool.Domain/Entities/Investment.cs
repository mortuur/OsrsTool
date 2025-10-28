namespace OsrsTool.Domain.Entities;

/// <summary>
/// Represents an OSRS item investment
/// </summary>
public class Investment
{
    public Guid Id { get; private set; }
    public string ItemName { get; private set; } = string.Empty;
    public int Quantity { get; private set; }
    public decimal PurchasePrice { get; private set; }
    public decimal CurrentPrice { get; private set; }
    public DateTime PurchaseDate { get; private set; }

    private Investment() { }

    public Investment(string itemName, int quantity, decimal purchasePrice, DateTime purchaseDate)
    {
        if (string.IsNullOrWhiteSpace(itemName))
            throw new ArgumentException("Item name cannot be empty", nameof(itemName));
        
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero", nameof(quantity));
        
        if (purchasePrice < 0)
            throw new ArgumentException("Purchase price cannot be negative", nameof(purchasePrice));

        Id = Guid.NewGuid();
        ItemName = itemName;
        Quantity = quantity;
        PurchasePrice = purchasePrice;
        CurrentPrice = purchasePrice;
        PurchaseDate = purchaseDate;
    }

    public void UpdateCurrentPrice(decimal currentPrice)
    {
        if (currentPrice < 0)
            throw new ArgumentException("Current price cannot be negative", nameof(currentPrice));
        
        CurrentPrice = currentPrice;
    }

    public decimal CalculateTotalInvestment() => Quantity * PurchasePrice;

    public decimal CalculateCurrentValue() => Quantity * CurrentPrice;

    public decimal CalculateProfitOrLoss() => CalculateCurrentValue() - CalculateTotalInvestment();

    public decimal CalculateProfitOrLossPercentage()
    {
        if (CalculateTotalInvestment() == 0)
            return 0;
        
        return (CalculateProfitOrLoss() / CalculateTotalInvestment()) * 100;
    }
}
