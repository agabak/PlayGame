using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Play.Catalog.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly ICatalogItemService _service;

        public ItemsController(ICatalogItemService service)
        {
            _service = service;
        }

        [HttpGet("api/{name}")]
        public string Get(string name)
        {
            return name;
        }

        [HttpGet]
        public async Task<IEnumerable<ItemDto>> Get()
        {
            return await _service.GetItemsAsync();
        }

        [HttpGet("{id}")]
        public async Task<ItemDto> Get(Guid id)
        {
            return await _service.GetItemById(id);
        }

        [HttpPost]
        public async Task<ActionResult<ItemDto>> Post(CreateItemDto createItem)
        {
            await _service.CreateItemAsync(createItem);
            return Ok(createItem);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(Guid id, UpdateItemDto updateItem)
        {
            await _service.UpdateItemAsync(id,updateItem);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _service.RemoveItemAsync(id);
            return NoContent();
        }
    }
}
