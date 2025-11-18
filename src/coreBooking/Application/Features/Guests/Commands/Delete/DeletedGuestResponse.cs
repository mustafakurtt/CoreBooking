using NArchitecture.Core.Application.Responses;

namespace Application.Features.Guests.Commands.Delete;

public class DeletedGuestResponse : IResponse
{
    public Guid Id { get; set; }
}