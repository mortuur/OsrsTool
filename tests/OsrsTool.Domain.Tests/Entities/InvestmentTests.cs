using OsrsTool.Domain.Entities;
using TUnit.Core;
using TUnit.Assertions;

namespace OsrsTool.Domain.Tests.Entities;

public class InvestmentTests
{
    [Test]
    public async Task Constructor_WithValidParameters_CreatesInvestment()
    {
        // Arrange
        var itemName = "Abyssal whip";
        var quantity = 10;
        var purchasePrice = 2500000m;
        var purchaseDate = DateTime.UtcNow;

        // Act
        var investment = new Investment(itemName, quantity, purchasePrice, purchaseDate);

        // Assert
        await Assert.That(investment.Id).IsNotEqualTo(Guid.Empty);
        await Assert.That(investment.ItemName).IsEqualTo(itemName);
        await Assert.That(investment.Quantity).IsEqualTo(quantity);
        await Assert.That(investment.PurchasePrice).IsEqualTo(purchasePrice);
        await Assert.That(investment.CurrentPrice).IsEqualTo(purchasePrice);
        await Assert.That(investment.PurchaseDate).IsEqualTo(purchaseDate);
    }

    [Test]
    public void Constructor_WithEmptyItemName_ThrowsArgumentException()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentException>(() => new Investment("", 10, 2500000m, DateTime.UtcNow));
    }

    [Test]
    public void Constructor_WithZeroQuantity_ThrowsArgumentException()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentException>(() => new Investment("Abyssal whip", 0, 2500000m, DateTime.UtcNow));
    }

    [Test]
    public void Constructor_WithNegativePrice_ThrowsArgumentException()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentException>(() => new Investment("Abyssal whip", 10, -100m, DateTime.UtcNow));
    }

    [Test]
    public async Task UpdateCurrentPrice_WithValidPrice_UpdatesPrice()
    {
        // Arrange
        var investment = new Investment("Abyssal whip", 10, 2500000m, DateTime.UtcNow);
        var newPrice = 2700000m;

        // Act
        investment.UpdateCurrentPrice(newPrice);

        // Assert
        await Assert.That(investment.CurrentPrice).IsEqualTo(newPrice);
    }

    [Test]
    public void UpdateCurrentPrice_WithNegativePrice_ThrowsArgumentException()
    {
        // Arrange
        var investment = new Investment("Abyssal whip", 10, 2500000m, DateTime.UtcNow);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => investment.UpdateCurrentPrice(-100m));
    }

    [Test]
    public async Task CalculateTotalInvestment_ReturnsCorrectValue()
    {
        // Arrange
        var investment = new Investment("Abyssal whip", 10, 2500000m, DateTime.UtcNow);

        // Act
        var total = investment.CalculateTotalInvestment();

        // Assert
        await Assert.That(total).IsEqualTo(25000000m);
    }

    [Test]
    public async Task CalculateCurrentValue_ReturnsCorrectValue()
    {
        // Arrange
        var investment = new Investment("Abyssal whip", 10, 2500000m, DateTime.UtcNow);
        investment.UpdateCurrentPrice(2700000m);

        // Act
        var value = investment.CalculateCurrentValue();

        // Assert
        await Assert.That(value).IsEqualTo(27000000m);
    }

    [Test]
    public async Task CalculateProfitOrLoss_WithProfit_ReturnsPositiveValue()
    {
        // Arrange
        var investment = new Investment("Abyssal whip", 10, 2500000m, DateTime.UtcNow);
        investment.UpdateCurrentPrice(2700000m);

        // Act
        var profitOrLoss = investment.CalculateProfitOrLoss();

        // Assert
        await Assert.That(profitOrLoss).IsEqualTo(2000000m);
    }

    [Test]
    public async Task CalculateProfitOrLoss_WithLoss_ReturnsNegativeValue()
    {
        // Arrange
        var investment = new Investment("Abyssal whip", 10, 2500000m, DateTime.UtcNow);
        investment.UpdateCurrentPrice(2300000m);

        // Act
        var profitOrLoss = investment.CalculateProfitOrLoss();

        // Assert
        await Assert.That(profitOrLoss).IsEqualTo(-2000000m);
    }

    [Test]
    public async Task CalculateProfitOrLossPercentage_WithProfit_ReturnsCorrectPercentage()
    {
        // Arrange
        var investment = new Investment("Abyssal whip", 10, 2500000m, DateTime.UtcNow);
        investment.UpdateCurrentPrice(2750000m);

        // Act
        var percentage = investment.CalculateProfitOrLossPercentage();

        // Assert
        await Assert.That(percentage).IsEqualTo(10m);
    }
}
