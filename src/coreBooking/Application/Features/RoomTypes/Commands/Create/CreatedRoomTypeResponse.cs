using NArchitecture.Core.Application.Responses;

namespace Application.Features.RoomTypes.Commands.Create;

public class CreatedRoomTypeResponse : IResponse
{
    public Guid Id { get; set; }
    public Guid HotelId { get; set; }
    public string Name { get; set; }
    public int Capacity { get; set; }
}