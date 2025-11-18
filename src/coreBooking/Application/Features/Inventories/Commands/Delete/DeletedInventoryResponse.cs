using NArchitecture.Core.Application.Responses;

namespace Application.Features.Inventories.Commands.Delete;

public class DeletedInventoryResponse : IResponse
{
    public Guid Id { get; set; }
}