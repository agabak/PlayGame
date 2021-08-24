using MassTransit;
using Play.Common;
using Play.Inventory.Entities;
using System.Threading.Tasks;

namespace Play.Inventory.Consumers
{
    public class CatalogItemUpdatedConsumer : IConsumer<CatalogItemUpdate>
    {
        private readonly IRepository<CatalogItem> _repository;
        public CatalogItemUpdatedConsumer(IRepository<CatalogItem> repository)
        {
            _repository = repository;
        }
        public async Task Consume(ConsumeContext<CatalogItemUpdate> context)
        {
            var message = context.Message;
            var item = await _repository.GetAsync(message.ItemId);
            if (item is null)
            {
                item = new CatalogItem
                {
                    Id = message.ItemId,
                    Name = message.Name,
                    Description = message.Description
                };
                await _repository.CreateAsync(item);

            }
            else
            {
                item.Name = message.Name;
                item.Description = message.Description;
                await _repository.UpdateAsync(item);
            }
        }
    }
}
