﻿using MassTransit;
using Play.Common;
using Play.Inventory.Entities;
using System.Threading.Tasks;

namespace Play.Inventory.Consumers
{
    public class CatalogItemDeletedConsumer: IConsumer<CatalogItemDeleted>
    {
        private readonly IRepository<CatalogItem> _repository;
        public CatalogItemDeletedConsumer(IRepository<CatalogItem> repository)
        {
            _repository = repository;
        }
        public async Task Consume(ConsumeContext<CatalogItemDeleted> context)
        {
            var message = context.Message;
            var item = await _repository.GetAsync(message.ItemId);
            if (item is null) return;
            await _repository.RemoveAsync(message.ItemId);
        }
    }
}
