using Play.Inventory.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Play.Inventory.Services
{
    public interface IInventoryItemService
    {
        Task<IEnumerable<InventoryItemDto>> GetAllItemsAsync(Guid userId);

        Task<InventoryItem> GrandItemsAsync(GrandItemsDto grandItemsDto);
    }
}
