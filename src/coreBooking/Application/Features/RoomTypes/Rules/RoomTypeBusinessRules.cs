using Application.Features.RoomTypes.Constants;
using Application.Services.Repositories;
using NArchitecture.Core.Application.Rules;
using NArchitecture.Core.CrossCuttingConcerns.Exception.Types;
using NArchitecture.Core.Localization.Abstraction;
using Domain.Entities;

namespace Application.Features.RoomTypes.Rules;

public class RoomTypeBusinessRules : BaseBusinessRules
{
    private readonly IRoomTypeRepository _roomTypeRepository;
    private readonly IHotelRepository _hotelRepository; // EKLENDÝ
    private readonly ILocalizationService _localizationService;

    public RoomTypeBusinessRules(
        IRoomTypeRepository roomTypeRepository,
        IHotelRepository hotelRepository, // EKLENDÝ
        ILocalizationService localizationService)
    {
        _roomTypeRepository = roomTypeRepository;
        _hotelRepository = hotelRepository;
        _localizationService = localizationService;
    }

    private async Task throwBusinessException(string messageKey)
    {
        string message = await _localizationService.GetLocalizedAsync(messageKey, RoomTypesBusinessMessages.SectionName);
        throw new BusinessException(message);
    }

    // 1. Oda Tipi Varlýk Kontrolü
    public async Task RoomTypeShouldExistWhenSelected(RoomType? roomType)
    {
        if (roomType == null)
            await throwBusinessException(RoomTypesBusinessMessages.RoomTypeNotExists);
    }

    public async Task RoomTypeIdShouldExistWhenSelected(Guid id, CancellationToken cancellationToken)
    {
        RoomType? roomType = await _roomTypeRepository.GetAsync(
            predicate: rt => rt.Id == id,
            enableTracking: false,
            cancellationToken: cancellationToken
        );
        await RoomTypeShouldExistWhenSelected(roomType);
    }

    // 2. YENÝ: Otel Varlýk Kontrolü
    public async Task HotelIdShouldExist(Guid hotelId, CancellationToken cancellationToken)
    {
        bool doesExist = await _hotelRepository.AnyAsync(
            predicate: h => h.Id == hotelId,
            cancellationToken: cancellationToken
        );

        if (!doesExist)
            await throwBusinessException(RoomTypesBusinessMessages.HotelNotExists);
    }
}   