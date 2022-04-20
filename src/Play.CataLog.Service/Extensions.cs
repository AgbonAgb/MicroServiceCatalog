using Play.CataLog.Service.Dtos;
using Play.CataLog.Service.Entities;

namespace Play.CataLog.Service
{
    public static class Extensions
    {

        public static ItemDto asDto(this Item item)
        {

            return new ItemDto(item.Id, item.Name, item.Description, item.Price, item.CreatedDate);
        }
    }

}