using NArchitecture.Core.Application.Dtos;
using Domain.Enums;

namespace Application.Features.Bookings.Queries.GetList;

public class GetListBookingListItemDto : IDto
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public Guid RoomTypeId { get; set; }
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
    public int NumberOfGuests { get; set; }
    public decimal TotalPrice { get; set; }
    public BookingStatus Status { get; set; }
}