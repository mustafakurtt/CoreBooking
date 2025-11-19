using NArchitecture.Core.Application.Dtos;

namespace Application.Features.RoomTypes.Queries.GetListByDynamic;

public class GetListByDynamicRoomTypeListItemDto : IDto
{
    public Guid Id { get; set; } // Listelerde iþlem yapmak için ID þarttýr
    public Guid HotelId { get; set; }
    public string Name { get; set; }
    public int Capacity { get; set; }

    // YENÝ: Ýliþkili Veri (Otel Adý)
    public string HotelName { get; set; }
}