using NArchitecture.Core.Application.Dtos;

namespace Application.Features.RoomTypes.Queries.GetList;

public class GetListRoomTypeListItemDto : IDto
{
    public Guid Id { get; set; }
    public Guid HotelId { get; set; }
    public string Name { get; set; }
    public int Capacity { get; set; }
}