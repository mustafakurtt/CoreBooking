using NArchitecture.Core.Application.Responses;

namespace Application.Features.Hotels.Commands.Delete;

public class DeletedHotelResponse : IResponse
{
    public Guid Id { get; set; }
}