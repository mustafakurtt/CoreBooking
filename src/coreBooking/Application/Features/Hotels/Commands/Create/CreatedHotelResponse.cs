using NArchitecture.Core.Application.Responses;

namespace Application.Features.Hotels.Commands.Create;

public class CreatedHotelResponse : IResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string City { get; set; }
    public string Address { get; set; }
}