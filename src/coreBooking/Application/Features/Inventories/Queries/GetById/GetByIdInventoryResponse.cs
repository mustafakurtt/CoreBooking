using NArchitecture.Core.Application.Responses;
using Shared.Enums;

namespace Application.Features.Inventories.Queries.GetById;

public class GetByIdInventoryResponse : IResponse
{
    public Guid Id { get; set; }
    public Guid RoomTypeId { get; set; }

    // --- Zenginleþtirilmiþ Veriler (Include ile gelecek) ---
    public string HotelName { get; set; }
    public string RoomTypeName { get; set; }

    public DateTime Date { get; set; }
    public int Quantity { get; set; }

    // --- V2: Flat Money Output ---
    public decimal Price { get; set; } // Amount
    public Currency PriceCurrency { get; set; } // TRY, USD
}