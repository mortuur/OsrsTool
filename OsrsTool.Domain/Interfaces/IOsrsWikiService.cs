using OsrsTracker.Domain.DTOs;
namespace OsrsTracker.Domain.Interfaces

{
    public interface IOsrsWikiService
    {
        Task<List<ItemDto>> GetItemMappingsAsync();
    }
}
