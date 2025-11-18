using NArchitecture.Core.Application.Dtos;

namespace Application.Features.Hotels.Queries.GetListByDynamic;

public class GetListByDynamicHotelListItemDto : IDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string City { get; set; }

    // --- Flat Address (Value Object Düzleþtirme) ---
    public string AddressStreet { get; set; }
    public string AddressCity { get; set; }
    public string AddressCountry { get; set; }
    public string AddressZipCode { get; set; }

    // --- Ekstra Bilgi ---
    public int RoomTypeCount { get; set; } // Kaç çeþit odasý var?
}