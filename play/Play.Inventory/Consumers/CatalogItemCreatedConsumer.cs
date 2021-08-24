using MassTransit;
using Play.Common;
using Play.Inventory.Entities;
using System.Threading.Tasks;

namespace Play.Inventory.Consumers
{
    public class CatalogItemCreatedConsumer : IConsumer<CatalogItemCreate>
    {
        private readonly IRepository<CatalogItem> _repository;

        public CatalogItemCreatedConsumer(
              IRepository<CatalogItem> repository)
        {
            _repository = repository;
        }
        public async Task Consume(ConsumeContext<CatalogItemCreate> context)
        {
            var message = context.Message;

            var item = await _repository.GetAsync(message.ItemId);
            if (item is not null) return;

            item = new CatalogItem
            {
                Id = message.ItemId,
                Name = message.Name,
                Description = message.Description
            };
            await _repository.CreateAsync(item);
        }
    }
}
