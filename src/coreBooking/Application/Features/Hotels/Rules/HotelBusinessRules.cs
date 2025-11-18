using Application.Features.Hotels.Constants;
using Application.Services.Repositories;
using NArchitecture.Core.Application.Rules;
using NArchitecture.Core.CrossCuttingConcerns.Exception.Types;
using NArchitecture.Core.Localization.Abstraction;
using Domain.Entities;

namespace Application.Features.Hotels.Rules;

public class HotelBusinessRules : BaseBusinessRules
{
    private readonly IHotelRepository _hotelRepository;
    private readonly ILocalizationService _localizationService;

    public HotelBusinessRules(IHotelRepository hotelRepository, ILocalizationService localizationService)
    {
        _hotelRepository = hotelRepository;
        _localizationService = localizationService;
    }

    private async Task throwBusinessException(string messageKey)
    {
        string message = await _localizationService.GetLocalizedAsync(messageKey, HotelsBusinessMessages.SectionName);
        throw new BusinessException(message);
    }

    // 1. Otel Varlýk Kontrolü
    public async Task HotelShouldExistWhenSelected(Hotel? hotel)
    {
        if (hotel == null)
            await throwBusinessException(HotelsBusinessMessages.HotelNotExists);
    }

    public async Task HotelIdShouldExistWhenSelected(Guid id, CancellationToken cancellationToken)
    {
        Hotel? hotel = await _hotelRepository.GetAsync(
            predicate: h => h.Id == id,
            enableTracking: false,
            cancellationToken: cancellationToken
        );
        await HotelShouldExistWhenSelected(hotel);
    }

    // 2. Ýsim Benzersizliði (Create Ýçin)
    public async Task HotelNameShouldNotExistsWhenInsert(string name, CancellationToken cancellationToken)
    {
        bool doesExist = await _hotelRepository.AnyAsync(
            predicate: h => h.Name == name,
            cancellationToken: cancellationToken
        );
        if (doesExist)
            await throwBusinessException(HotelsBusinessMessages.HotelNameAlreadyExists);
    }

    // 3. Ýsim Benzersizliði (Update Ýçin - Kendi adý hariç)
    public async Task HotelNameShouldNotExistsWhenUpdate(Guid id, string name, CancellationToken cancellationToken)
    {
        bool doesExist = await _hotelRepository.AnyAsync(
            predicate: h => h.Id != id && h.Name == name,
            cancellationToken: cancellationToken
        );
        if (doesExist)
            await throwBusinessException(HotelsBusinessMessages.HotelNameAlreadyExists);
    }
}