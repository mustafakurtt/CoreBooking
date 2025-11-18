using NArchitecture.Core.Application.Responses;

namespace Application.Features.Guests.Queries.GetById;

public class GetByIdGuestResponse : IResponse
{
    public Guid Id { get; set; }
    public Guid BookingId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string Nationality { get; set; }
    public bool IsPrimary { get; set; }
}