using OsrsTool.Domain.DTOs;
namespace OsrsTool.Domain.Interfaces

{
    public interface IOsrsWikiService
    {
        Task<List<ItemDto>> GetItemMappingsAsync();
    }
}
