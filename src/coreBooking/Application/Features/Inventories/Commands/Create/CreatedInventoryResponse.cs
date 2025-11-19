using NArchitecture.Core.Application.Responses;
using Shared.Enums;

namespace Application.Features.Inventories.Commands.Create;

public class CreatedInventoryResponse : IResponse
{
    public Guid Id { get; set; }
    public Guid RoomTypeId { get; set; }
    public DateTime Date { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; } 
    public Currency PriceCurrency { get; set; } 
}