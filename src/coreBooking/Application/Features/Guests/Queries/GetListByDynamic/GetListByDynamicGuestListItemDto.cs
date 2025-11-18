using NArchitecture.Core.Application.Dtos;

namespace Application.Features.Guests.Queries.GetListByDynamic;

public class GetListByDynamicGuestListItemDto : IDto
{
    public Guid BookingId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string Nationality { get; set; }
    public bool IsPrimary { get; set; }
}
