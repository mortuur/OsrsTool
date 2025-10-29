namespace OsrsTracker.Domain.Entities
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Examine { get; set; } = string.Empty;
        public bool MembersOnly { get; set; }
        public int? Limit { get; set; }
        public int? HighPrice { get; set; }
        public int? LowPrice { get; set; }
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
    }
}
