using OsrsTool.Domain.Enums;

namespace OsrsTool.Domain.DTOs
{
    public class GearUpgradeDto
    {
        public CombatStyle CombatStyle { get; set; }
        public GearSlot Slot { get; set; }
        public GearItemDto? CurrentItem { get; set; }
        public GearItemDto? NextUpgrade { get; set; }
        public long UpgradeCost { get; set; }
        public long NetCost { get; set; } // Cost after selling current item
    }
}
