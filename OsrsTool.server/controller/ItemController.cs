using Microsoft.AspNetCore.Mvc;
using OsrsTool.Domain.Interfaces;

namespace OsrsTool.Server.controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly IItemService _itemService;
        public ItemController(IItemService itemService)
        {
            _itemService = itemService;
        }

        [HttpGet("test")]
        public IActionResult Test()
        {
            return Ok("ItemController is working!");
        }

        [HttpGet]
        public async Task<IActionResult> GetAllItems()
        {
            var items = await _itemService.GetAllItemsAsync();
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetItem(int id)
        {
            var item = await _itemService.GetItemByIdAsync(id);
            return item == null ? NotFound() : Ok(item);
        }
    }
}