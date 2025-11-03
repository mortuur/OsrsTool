using OsrsTool.Domain.Enums;

namespace OsrsTool.Domain.DTOs
{
    public class GearItemDto
    {
        public int ItemId { get; set; }
        public string Name { get; set; } = string.Empty;
        public GearSlot Slot { get; set; }
        public int Tier { get; set; }
        public long? CurrentPrice { get; set; }
    }
}
