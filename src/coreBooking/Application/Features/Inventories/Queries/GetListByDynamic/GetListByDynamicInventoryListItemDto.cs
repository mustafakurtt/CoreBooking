using NArchitecture.Core.Application.Dtos;
using Shared.Enums;

namespace Application.Features.Inventories.Queries.GetListByDynamic;

public class GetListByDynamicInventoryListItemDto : IDto
{
    public Guid Id { get; set; } // ID'yi de ekleyelim, iþlem yapmak gerekebilir
    public Guid RoomTypeId { get; set; }

    // --- Zenginleþtirilmiþ Veri (Include ile gelecek) ---
    public string HotelName { get; set; }
    public string RoomTypeName { get; set; }

    public DateTime Date { get; set; }
    public int Quantity { get; set; }

    // --- V2: Money Flattening ---
    public decimal Price { get; set; }        // Money.Amount
    public Currency PriceCurrency { get; set; } // Money.Currency
}