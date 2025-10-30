namespace OsrsTool.Domain.Entities.Api
{
    public class OsrsItemMapping
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Examine { get; set; } = string.Empty;
        public bool Members { get; set; }
        public int? Limit { get; set; }
        public string Icon { get; set; } = string.Empty;
    }
}
