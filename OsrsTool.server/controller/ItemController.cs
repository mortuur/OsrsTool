using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OsrsTool.Domain.DTOs;

namespace OsrsTool.Server.controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        [HttpGet("test")]
        public IActionResult Test()
        {
            return Ok("ItemController is working!");
        }

        [HttpGet]
        public IActionResult GetAllItems()
        {
            var items = new List<ItemDto>
            {
                new ItemDto { Id = 1, Name = "Item 1", Examine = "This is item 1.", Members = false },
                new ItemDto { Id = 2, Name = "Item 2", Examine = "This is item 2.", Members = true }
            };
            return Ok(items);
        }

        [HttpGet("{id}")]
        public IActionResult GetItem(int id)
        {
            var item = new ItemDto { Id = id, Name = "Sample Item", Examine = "This is a sample item.", Members = false };
            return Ok(item);
        }
    }
}