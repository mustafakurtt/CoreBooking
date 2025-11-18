using NArchitecture.Core.Application.Dtos;

namespace Application.Features.RoomTypes.Queries.GetListByDynamic;

public class GetListByDynamicRoomTypeListItemDto : IDto
{
    public Guid HotelId { get; set; }
    public string Name { get; set; }
    public int Capacity { get; set; }
}
