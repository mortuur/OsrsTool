using OsrsTool.Domain.Entities;
using OsrsTool.Domain.Entities.Api;
using OsrsTool.Domain.Interfaces;
using System.Net.Http.Json;
using System.Linq;

namespace OsrsTool.Infrastructure.Services
{
    public class OsrsApiService : IOsrsApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IRepository<Item> _itemRepo;
        private readonly IRepository<ItemPriceHistory> _historyRepo;

        public OsrsApiService(HttpClient httpClient,
                              IRepository<Item> itemRepo,
                              IRepository<ItemPriceHistory> historyRepo)
        {
            _httpClient = httpClient;
            _itemRepo = itemRepo;
            _historyRepo = historyRepo;
        }

        public async Task FetchAndStoreItemsAsync()
        {
            var url = "https://prices.runescape.wiki/api/v1/osrs/mapping";
            var apiItems = await _httpClient.GetFromJsonAsync<List<OsrsItemMapping>>(url);

            if (apiItems == null) return;

            var dbItems = await _itemRepo.GetAllAsync();
            // Materialize into a dictionary for O(1) lookups
            var itemsById = dbItems.ToDictionary(i => i.Id);

            foreach (var apiItem in apiItems)
            {
                if (itemsById.TryGetValue(apiItem.Id, out var existing))
                {
                    existing.Name = apiItem.Name;
                    existing.Examine = apiItem.Examine;
                    existing.MembersOnly = apiItem.Members;
                    existing.Limit = apiItem.Limit;
                    existing.LastUpdated = DateTime.UtcNow;
                    _itemRepo.Update(existing);
                }
                else
                {
                    var newItem = new Item
                    {
                        Id = apiItem.Id,
                        Name = apiItem.Name,
                        Examine = apiItem.Examine,
                        MembersOnly = apiItem.Members,
                        Limit = apiItem.Limit,
                        LastUpdated = DateTime.UtcNow
                    };

                    await _itemRepo.AddAsync(newItem);
                    // Keep dictionary consistent in case mapping contains duplicates
                    itemsById[newItem.Id] = newItem;
                }
            }

            await _itemRepo.SaveChangesAsync();
        }

        public async Task FetchAndStoreLatestPricesAsync()
        {
            var url = "https://prices.runescape.wiki/api/v1/osrs/latest";
            var response = await _httpClient.GetFromJsonAsync<OsrsPriceResponse>(url);
            if (response == null || response.Data.Count == 0) return;

            var dbItems = await _itemRepo.GetAllAsync();
            
            var itemsById = dbItems.ToDictionary(i => i.Id);

            foreach (var kv in response.Data)
            {
                if (!int.TryParse(kv.Key, out int itemId)) continue;
                var price = kv.Value;

                if (!itemsById.TryGetValue(itemId, out var item)) continue;

                await _historyRepo.AddAsync(new ItemPriceHistory
                {
                    ItemId = itemId,
                    HighPrice = price.High ?? 0,
                    LowPrice = price.Low ?? 0,
                    RecordedAt = DateTime.UtcNow
                });
                item.LastUpdated = DateTime.UtcNow;

            }

            await _itemRepo.SaveChangesAsync();
            await _historyRepo.SaveChangesAsync();
        }
    }
}
