using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OsrsTool.Domain.Entities
{
    public class Item
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Examine { get; set; } = string.Empty;
        public bool MembersOnly { get; set; }
        public int? Limit { get; set; }
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;

        public ICollection<ItemPriceHistory> PriceHistory { get; set; } = new List<ItemPriceHistory>();

        [NotMapped]
        public ItemPriceHistory? LatestPrice => PriceHistory
            .OrderByDescending(p => p.RecordedAt)
            .FirstOrDefault();
    }

}
