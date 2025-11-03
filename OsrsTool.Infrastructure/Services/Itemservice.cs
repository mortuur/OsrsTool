using OsrsTool.Domain.DTOs;
using OsrsTool.Domain.Interfaces;
using OsrsTool.Domain.Entities;

namespace OsrsTool.Infrastructure.Services
{
    public class Itemservice : IItemService
    {
        public readonly IRepository<Item> _itemRepository;

        public Itemservice(IRepository<Item> itemRepository)
        {
            _itemRepository = itemRepository;
        }

        public async Task<List<ItemDto>> GetAllItemsAsync()
        {
            var items = await _itemRepository.GetAllAsync();
            return items.Select(item => new ItemDto
            {
                Id = item.Id,
                Name = item.Name,
                Examine = item.Examine,
                Members = item.Members
            }).ToList();
        }

        public async Task<ItemDto?> GetItemByIdAsync(int id)
        {
            var item = await _itemRepository.GetByIdAsync(id);
            // check for null and map to DTO
            return item == null ? null : new ItemDto
            {
                Id = item.Id,
                Name = item.Name,
                Examine = item.Examine,
                Members = item.Members
            };
        }
    }
}
