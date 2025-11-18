using NArchitecture.Core.Application.Dtos;

namespace Application.Features.Inventories.Queries.GetListByDynamic;

public class GetListByDynamicInventoryListItemDto : IDto
{
    public Guid RoomTypeId { get; set; }
    public DateTime Date { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}
