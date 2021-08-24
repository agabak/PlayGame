using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Play.Catalog.Services
{
    public interface ICatalogItemService
    {
        Task<IEnumerable<ItemDto>> GetItemsAsync();

        Task<ItemDto> GetItemById(Guid id);

        Task CreateItemAsync(CreateItemDto createItem);

        Task UpdateItemAsync(Guid id, UpdateItemDto updateItem);

        Task RemoveItemAsync(Guid id);
    }
}
