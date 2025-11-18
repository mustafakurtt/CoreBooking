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
    private readonly ILocalizationService _localizationService;

    public RoomTypeBusinessRules(IRoomTypeRepository roomTypeRepository, ILocalizationService localizationService)
    {
        _roomTypeRepository = roomTypeRepository;
        _localizationService = localizationService;
    }

    private async Task throwBusinessException(string messageKey)
    {
        string message = await _localizationService.GetLocalizedAsync(messageKey, RoomTypesBusinessMessages.SectionName);
        throw new BusinessException(message);
    }

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
}