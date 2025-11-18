using NArchitecture.Core.Application.Dtos;

namespace Application.Features.Guests.Queries.GetList;

public class GetListGuestListItemDto : IDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Nationality { get; set; }
    public bool IsPrimary { get; set; }
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
    public string RoomTypeName { get; set; }
    public string HotelName { get; set; }
    public string HotelCity { get; set; }
}