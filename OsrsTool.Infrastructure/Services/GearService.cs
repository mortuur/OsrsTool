using System.Text.Json;
using Microsoft.Extensions.Hosting;
using OsrsTool.Domain.DTOs;
using OsrsTool.Domain.Enums;
using OsrsTool.Domain.Interfaces;
using OsrsTool.Domain.Entities;

namespace OsrsTool.Infrastructure.Services
{
    public class GearService : IGearService
    {
        private readonly IRepository<Item> _itemRepository;
        private readonly string _bisGearDataPath;
        private Dictionary<string, Dictionary<string, List<JsonElement>>>? _bisGearData;

        public GearService(IRepository<Item> itemRepository, IHostEnvironment env)
        {
            _itemRepository = itemRepository;
            _bisGearDataPath = Path.Combine(env.ContentRootPath, "Data", "BisGearData.json");
        }

        private async Task LoadBisGearDataAsync()
        {
            if (_bisGearData == null)
            {
                var json = await File.ReadAllTextAsync(_bisGearDataPath);
                _bisGearData = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, List<JsonElement>>>>(json);
            }
        }

        public async Task<Dictionary<GearSlot, List<GearItemDto>>> GetBisProgressionAsync(CombatStyle combatStyle)
        {
            await LoadBisGearDataAsync();
            
            var result = new Dictionary<GearSlot, List<GearItemDto>>();
            
            if (_bisGearData == null || !_bisGearData.ContainsKey(combatStyle.ToString()))
                return result;

            var styleData = _bisGearData[combatStyle.ToString()];
            
            foreach (var slotEntry in styleData)
            {
                if (Enum.TryParse<GearSlot>(slotEntry.Key, out var slot))
                {
                    var items = new List<GearItemDto>();
                    
                    foreach (var itemElement in slotEntry.Value)
                    {
                        var itemId = itemElement.GetProperty("itemId").GetInt32();
                        var name = itemElement.GetProperty("name").GetString() ?? "";
                        var tier = itemElement.GetProperty("tier").GetInt32();
                        
                        // Try to get current price from database
                        var dbItem = await _itemRepository.GetByIdAsync(itemId);
                        long? currentPrice = dbItem?.LatestPrice?.HighPrice;
                        
                        items.Add(new GearItemDto
                        {
                            ItemId = itemId,
                            Name = name,
                            Slot = slot,
                            Tier = tier,
                            CurrentPrice = currentPrice
                        });
                    }
                    
                    result[slot] = items;
                }
            }
            
            return result;
        }

        public async Task<List<GearUpgradeDto>> GetGearUpgradesAsync(CombatStyle combatStyle, List<int> ownedItemIds)
        {
            var bisProgression = await GetBisProgressionAsync(combatStyle);
            var upgrades = new List<GearUpgradeDto>();

            foreach (var slotEntry in bisProgression)
            {
                var slot = slotEntry.Key;
                var items = slotEntry.Value.OrderBy(i => i.Tier).ToList();

                // Find current item (highest tier owned)
                GearItemDto? currentItem = null;
                foreach (var item in items)
                {
                    if (ownedItemIds.Contains(item.ItemId))
                    {
                        if (currentItem == null || item.Tier > currentItem.Tier)
                        {
                            currentItem = item;
                        }
                    }
                }

                // Find next upgrade (next tier after current, or first tier if no item owned)
                GearItemDto? nextUpgrade = null;
                if (currentItem != null)
                {
                    nextUpgrade = items.FirstOrDefault(i => i.Tier > currentItem.Tier);
                }
                else
                {
                    nextUpgrade = items.FirstOrDefault();
                }

                // Only add if there's an upgrade available
                if (nextUpgrade != null)
                {
                    var upgradeCost = nextUpgrade.CurrentPrice ?? 0;
                    var currentValue = currentItem?.CurrentPrice ?? 0;
                    var netCost = upgradeCost - currentValue;

                    upgrades.Add(new GearUpgradeDto
                    {
                        CombatStyle = combatStyle,
                        Slot = slot,
                        CurrentItem = currentItem,
                        NextUpgrade = nextUpgrade,
                        UpgradeCost = upgradeCost,
                        NetCost = netCost
                    });
                }
            }

            return upgrades.OrderBy(u => u.NetCost).ToList();
        }
    }
}
