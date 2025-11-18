using NArchitecture.Core.Application.Responses;

namespace Application.Features.RoomTypes.Commands.Delete;

public class DeletedRoomTypeResponse : IResponse
{
    public Guid Id { get; set; }
}