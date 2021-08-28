using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Play.Inventory.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Play.Inventory.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly IInventoryItemService _service;

        public ItemsController(IInventoryItemService service)
        {
            _service = service;    
        }

        [HttpGet("api/{name}")]
        public string Get(string name)
        {
            return name;
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<IEnumerable<InventoryItemDto>>> GetAsync(Guid userId)
        {
            
            if (userId == Guid.Empty) return BadRequest();

            return Ok(await _service.GetAllItemsAsync(userId));
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync(GrandItemsDto grandItemsDto)
        {
            if (grandItemsDto is null) return BadRequest();
            return Ok(await _service.GrandItemsAsync(grandItemsDto));
        }
    }
}
