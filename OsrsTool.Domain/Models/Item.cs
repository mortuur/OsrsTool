namespace OsrsTool.Domain.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public long BuyPrice { get; set; }
        public long SellPrice { get; set; }
        public bool Members { get; set; }

        public string Examine { get; set; } = "";

        public DateTime LastUpdated { get; set; }
    }

}
