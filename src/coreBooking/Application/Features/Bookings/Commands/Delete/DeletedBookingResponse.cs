using NArchitecture.Core.Application.Responses;

namespace Application.Features.Bookings.Commands.Delete;

public class DeletedBookingResponse : IResponse
{
    public Guid Id { get; set; }
}