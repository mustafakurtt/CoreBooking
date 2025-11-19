using NArchitecture.Core.Application.Responses;
using Shared.Enums;

namespace Application.Features.Bookings.Commands.Cancel;

public class CanceledBookingResponse : IResponse
{
    public Guid Id { get; set; }
    public BookingStatus Status { get; set; }
    public DateTime UpdatedDate { get; set; }
}