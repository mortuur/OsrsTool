using OsrsTracker.Domain.Interfaces;
using System.Net.Http.Json;

namespace OsrsTracker.Infrastructure.Services
{
    public class OsrsWikiService : IOsrsWikiService
    {
        private readonly HttpClient _http;

        public OsrsWikiService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<ItemDto>> GetItemMappingsAsync()
        {
            var url = "https://prices.runescape.wiki/api/v1/osrs/mapping";
            var items = await _http.GetFromJsonAsync<List<ItemDto>>(url);
            return items ?? new List<ItemDto>();
        }
    }
}
