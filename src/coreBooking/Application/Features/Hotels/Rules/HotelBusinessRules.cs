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
}