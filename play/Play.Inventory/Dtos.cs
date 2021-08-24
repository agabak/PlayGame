using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Play.Inventory
{
    public record GrandItemsDto(Guid Userid, Guid CatalogItemId, int Quantity);

    public record InventoryItemDto(Guid CatalogItemId,
          string Name, string Description, int Quantity, DateTimeOffset AcquiredDate);

    public record CatalogItemDto(Guid Id, string Name, string Description);

    public record CatalogItemCreate(Guid ItemId, string Name, string Description);

    public record CatalogItemUpdate(Guid ItemId, string Name, string Description);

    public record CatalogItemDeleted(Guid ItemId);
}
