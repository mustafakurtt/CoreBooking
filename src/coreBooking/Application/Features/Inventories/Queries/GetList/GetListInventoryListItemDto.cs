using NArchitecture.Core.Application.Dtos;

namespace Application.Features.Inventories.Queries.GetList;

public class GetListInventoryListItemDto : IDto
{
    public Guid Id { get; set; }
    public Guid RoomTypeId { get; set; }
    public DateTime Date { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}