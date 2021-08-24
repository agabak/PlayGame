using Play.Common;
using Play.Inventory.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Play.Inventory.Services
{
    public class InventoryItemService: IInventoryItemService
    {
        private readonly IRepository<InventoryItem> _repoInventory;
        private readonly IRepository<CatalogItem> _repoCatalog;

        public InventoryItemService(
            IRepository<InventoryItem> repoInventory,
            IRepository<CatalogItem> repoCatalog)
        {
            _repoInventory = repoInventory;
            _repoCatalog = repoCatalog;
        }

        public async Task<IEnumerable<InventoryItemDto>> GetAllItemsAsync(Guid userId)
        {
            if (userId == Guid.Empty) return null;

            var inventoryItemEntites = (await _repoInventory.GetAllAsync(item => item.UserId == userId)).ToList();
            var itemIds = inventoryItemEntites.Select(item => item.CatalogItemId);

            var catalogItems = await _repoCatalog.GetAllAsync(item => itemIds.Contains(item.Id));
            if (!catalogItems.Any()) return null;

            var inventoryItemDtos = inventoryItemEntites.Select(inventoryItem =>
            {
                var catalogItem =
                catalogItems.Single(catalogItem => catalogItem.Id == inventoryItem.CatalogItemId);
                return inventoryItem.AsDto(catalogItem.Name, catalogItem.Description);
            });

            return inventoryItemDtos;
        }

        public async Task<InventoryItem> GrandItemsAsync(GrandItemsDto grandItemsDto)
        {
            var inventoryItem = await _repoInventory
             .GetAsync(item => item.UserId == grandItemsDto.Userid && item.CatalogItemId == grandItemsDto.CatalogItemId);

            if (inventoryItem is null)
            {
                inventoryItem = new InventoryItem
                {
                    CatalogItemId = grandItemsDto.CatalogItemId,
                    UserId = grandItemsDto.Userid,
                    Quantity = grandItemsDto.Quantity,
                    AcquiredDate = DateTimeOffset.UtcNow
                };

                await _repoInventory.CreateAsync(inventoryItem);
            }
            else
            {
                inventoryItem.Quantity += grandItemsDto.Quantity;
                await _repoInventory.UpdateAsync(inventoryItem);
            }

            return inventoryItem;
        }
    }
}
