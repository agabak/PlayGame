using Play.Catalog.Entities;

namespace Play.Catalog
{
    public static class Extensions
    {
        public static ItemDto AsDtos(this Item item)
        {
            return new
            ItemDto(item.Id, item.Name, item.Description, item.Price, item.CreatedDate);
        }
    }
}
