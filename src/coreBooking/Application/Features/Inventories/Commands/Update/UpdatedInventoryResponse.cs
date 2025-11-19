using NArchitecture.Core.Application.Responses;
using Shared.Enums;

namespace Application.Features.Inventories.Commands.Update;

public class UpdatedInventoryResponse : IResponse
{
    public Guid Id { get; set; }
    public Guid RoomTypeId { get; set; }
    public DateTime Date { get; set; }
    public int Quantity { get; set; }

    // Flat Output
    public decimal Price { get; set; }
    public Currency PriceCurrency { get; set; } // Opsiyonel ama bilgi amaçlý iyi olur
}