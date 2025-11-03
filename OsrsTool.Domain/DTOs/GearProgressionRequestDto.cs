using OsrsTool.Domain.Enums;

namespace OsrsTool.Domain.DTOs
{
    public class GearProgressionRequestDto
    {
        public CombatStyle CombatStyle { get; set; }
        public List<int> OwnedItemIds { get; set; } = new();
    }
}
