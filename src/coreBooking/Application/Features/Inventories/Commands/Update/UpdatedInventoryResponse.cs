using NArchitecture.Core.Application.Responses;

namespace Application.Features.Inventories.Commands.Update;

public class UpdatedInventoryResponse : IResponse
{
    public Guid Id { get; set; }
    public Guid RoomTypeId { get; set; }
    public DateTime Date { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}