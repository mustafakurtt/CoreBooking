using NArchitecture.Core.Application.Responses;

namespace Application.Features.Hotels.Commands.Update;

public class UpdatedHotelResponse : IResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string City { get; set; }
    public string AddressStreet { get; set; }
    public string AddressCity { get; set; }
    public string AddressCountry { get; set; }
    public string AddressZipCode { get; set; }
}