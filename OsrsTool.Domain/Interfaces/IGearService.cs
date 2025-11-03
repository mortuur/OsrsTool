using OsrsTool.Domain.DTOs;
using OsrsTool.Domain.Enums;

namespace OsrsTool.Domain.Interfaces
{
    public interface IGearService
    {
        Task<List<GearUpgradeDto>> GetGearUpgradesAsync(CombatStyle combatStyle, List<int> ownedItemIds);
        Task<Dictionary<GearSlot, List<GearItemDto>>> GetBisProgressionAsync(CombatStyle combatStyle);
    }
}
