using Shared.Enums;

namespace Application.Features.RoomTypes.Queries.GetAvailableRooms;

public class GetAvailableRoomsResponse
{
    public Guid RoomTypeId { get; set; }
    public Guid HotelId { get; set; }
    public string HotelName { get; set; }
    public string HotelCity { get; set; }
    public string RoomTypeName { get; set; }
    public int Capacity { get; set; }

    // Flattened Money
    public decimal TotalPriceAmount { get; set; }
    public Currency Currency { get; set; }
}