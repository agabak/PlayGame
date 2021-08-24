using MassTransit;
using Play.Catalog.Entities;
using Play.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Play.Catalog.Services
{
    public class CatalogItemService: ICatalogItemService
    {
        private readonly IRepository<Item> _repository;
        private readonly IPublishEndpoint _publish;

        public CatalogItemService(
            IRepository<Item> repository,
            IPublishEndpoint  publish)
        {
            _repository = repository;
            _publish = publish;
        }

        public async Task<IEnumerable<ItemDto>> GetItemsAsync()
        {
            return (await _repository.GetAllAsync()).Select(item => item.AsDtos());
        }

        public async Task<ItemDto> GetItemById(Guid id)
        {
            return (await _repository.GetAsync(id)).AsDtos();
        }

        public async Task CreateItemAsync(CreateItemDto createItem)
        {
            Item item = new()
            {
                Name = createItem.Name,
                Description = createItem.Description,
                Price = createItem.Price,
                CreatedDate = DateTimeOffset.UtcNow
            };
            await _repository.CreateAsync(item);
            await _publish.Publish(new CatalogItemCreate(item.Id, item.Name, item.Description));
        }

        public async Task UpdateItemAsync(Guid id, UpdateItemDto updateItem)
        {
            var item = await _repository.GetAsync(id);
            if (item == null) return;

            item.Name = updateItem.Name ?? item.Name;
            item.Description = updateItem.Description ?? item.Description;
            item.Price = updateItem.Price > 0 ? updateItem.Price : item.Price;

            await _repository.UpdateAsync(item);
            await _publish.Publish(new CatalogItemUpdate(item.Id, item.Name, item.Description));
        }

        public async Task RemoveItemAsync(Guid id)
        {
            var item = await _repository.GetAsync(id);
            if (item is null) return;

            await _repository.RemoveAsync(item.Id);
            await _publish.Publish(new CatalogItemDeleted(item.Id));
        }
    }
}
