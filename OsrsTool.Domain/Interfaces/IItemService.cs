using OsrsTool.Domain.DTOs;

namespace OsrsTool.Domain.Interfaces
{
    public interface IItemService
    {
        Task<List<ItemDto>> GetAllItemsAsync();
        Task<ItemDto?> GetItemByIdAsync(int id);
    }
}
