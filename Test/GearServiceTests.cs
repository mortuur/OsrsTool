using OsrsTool.Domain.Enums;
using OsrsTool.Domain.Interfaces;
using OsrsTool.Infrastructure.Services;
using Moq;
using Xunit;
using OsrsTool.Domain.Entities;
using Microsoft.Extensions.Hosting;

namespace Test
{
    public class GearServiceTests
    {
        [Fact]
        public async Task GetBisProgression_ReturnsMeleeGearProgression()
        {
            // Arrange
            var mockItemRepository = new Mock<IRepository<Item>>();
            var mockEnvironment = new Mock<IHostEnvironment>();
            
            mockEnvironment.Setup(e => e.ContentRootPath)
                .Returns("/home/runner/work/OsrsTool/OsrsTool/OsrsTool.server");
            
            var gearService = new GearService(mockItemRepository.Object, mockEnvironment.Object);

            // Act
            var progression = await gearService.GetBisProgressionAsync(CombatStyle.Melee);

            // Assert
            Assert.NotEmpty(progression);
            Assert.True(progression.ContainsKey(GearSlot.Head));
            Assert.True(progression.ContainsKey(GearSlot.Body));
            Assert.True(progression.ContainsKey(GearSlot.Weapon));
        }

        [Fact]
        public async Task GetBisProgression_ReturnsRangeGearProgression()
        {
            // Arrange
            var mockItemRepository = new Mock<IRepository<Item>>();
            var mockEnvironment = new Mock<IHostEnvironment>();
            
            mockEnvironment.Setup(e => e.ContentRootPath)
                .Returns("/home/runner/work/OsrsTool/OsrsTool/OsrsTool.server");
            
            var gearService = new GearService(mockItemRepository.Object, mockEnvironment.Object);

            // Act
            var progression = await gearService.GetBisProgressionAsync(CombatStyle.Range);

            // Assert
            Assert.NotEmpty(progression);
            Assert.True(progression.ContainsKey(GearSlot.Head));
            Assert.True(progression.ContainsKey(GearSlot.Weapon));
        }

        [Fact]
        public async Task GetGearUpgrades_WithNoOwnedItems_ReturnsAllStarterUpgrades()
        {
            // Arrange
            var mockItemRepository = new Mock<IRepository<Item>>();
            var mockEnvironment = new Mock<IHostEnvironment>();
            
            mockEnvironment.Setup(e => e.ContentRootPath)
                .Returns("/home/runner/work/OsrsTool/OsrsTool/OsrsTool.server");
            
            var gearService = new GearService(mockItemRepository.Object, mockEnvironment.Object);

            // Act
            var upgrades = await gearService.GetGearUpgradesAsync(CombatStyle.Melee, new List<int>());

            // Assert
            Assert.NotEmpty(upgrades);
            // All upgrades should have no current item since we own nothing
            Assert.All(upgrades, upgrade => Assert.Null(upgrade.CurrentItem));
            // All upgrades should have a next upgrade (first tier item)
            Assert.All(upgrades, upgrade => Assert.NotNull(upgrade.NextUpgrade));
        }

        [Fact]
        public async Task GetGearUpgrades_WithOwnedItems_ReturnsCorrectNextUpgrades()
        {
            // Arrange
            var mockItemRepository = new Mock<IRepository<Item>>();
            var mockEnvironment = new Mock<IHostEnvironment>();
            
            mockEnvironment.Setup(e => e.ContentRootPath)
                .Returns("/home/runner/work/OsrsTool/OsrsTool/OsrsTool.server");
            
            var gearService = new GearService(mockItemRepository.Object, mockEnvironment.Object);
            
            // Own Iron full helm (itemId: 1153, tier 1)
            var ownedItemIds = new List<int> { 1153 };

            // Act
            var upgrades = await gearService.GetGearUpgradesAsync(CombatStyle.Melee, ownedItemIds);

            // Assert
            var headUpgrade = upgrades.FirstOrDefault(u => u.Slot == GearSlot.Head);
            Assert.NotNull(headUpgrade);
            Assert.NotNull(headUpgrade.CurrentItem);
            Assert.Equal(1153, headUpgrade.CurrentItem.ItemId);
            Assert.NotNull(headUpgrade.NextUpgrade);
            // Should recommend Steel full helm (tier 2) as next upgrade
            Assert.True(headUpgrade.NextUpgrade.Tier > headUpgrade.CurrentItem.Tier);
        }
    }
}
