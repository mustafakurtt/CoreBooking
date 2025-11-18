using Application.Features.Guests.Constants;
using Application.Services.Repositories;
using NArchitecture.Core.Application.Rules;
using NArchitecture.Core.CrossCuttingConcerns.Exception.Types;
using NArchitecture.Core.Localization.Abstraction;
using Domain.Entities;

namespace Application.Features.Guests.Rules;

public class GuestBusinessRules : BaseBusinessRules
{
    private readonly IGuestRepository _guestRepository;
    private readonly ILocalizationService _localizationService;

    public GuestBusinessRules(IGuestRepository guestRepository, ILocalizationService localizationService)
    {
        _guestRepository = guestRepository;
        _localizationService = localizationService;
    }

    private async Task throwBusinessException(string messageKey)
    {
        string message = await _localizationService.GetLocalizedAsync(messageKey, GuestsBusinessMessages.SectionName);
        throw new BusinessException(message);
    }

    public async Task GuestShouldExistWhenSelected(Guest? guest)
    {
        if (guest == null)
            await throwBusinessException(GuestsBusinessMessages.GuestNotExists);
    }

    public async Task GuestIdShouldExistWhenSelected(Guid id, CancellationToken cancellationToken)
    {
        Guest? guest = await _guestRepository.GetAsync(
            predicate: g => g.Id == id,
            enableTracking: false,
            cancellationToken: cancellationToken
        );
        await GuestShouldExistWhenSelected(guest);
    }
}