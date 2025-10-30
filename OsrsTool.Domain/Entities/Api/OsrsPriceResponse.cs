namespace OsrsTool.Domain.Entities.Api
{
    public class OsrsPriceResponse
    {
        public Dictionary<string, OsrsItemPrice> Data { get; set; } = new();
    }
}
