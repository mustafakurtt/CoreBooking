using Application.Features.Inventories.Constants;
using Application.Services.Repositories;
using NArchitecture.Core.Application.Rules;
using NArchitecture.Core.CrossCuttingConcerns.Exception.Types;
using NArchitecture.Core.Localization.Abstraction;
using Domain.Entities;

namespace Application.Features.Inventories.Rules;

public class InventoryBusinessRules : BaseBusinessRules
{
    private readonly IInventoryRepository _inventoryRepository;
    private readonly ILocalizationService _localizationService;

    public InventoryBusinessRules(IInventoryRepository inventoryRepository, ILocalizationService localizationService)
    {
        _inventoryRepository = inventoryRepository;
        _localizationService = localizationService;
    }

    private async Task throwBusinessException(string messageKey)
    {
        string message = await _localizationService.GetLocalizedAsync(messageKey, InventoriesBusinessMessages.SectionName);
        throw new BusinessException(message);
    }

    public async Task InventoryShouldExistWhenSelected(Inventory? inventory)
    {
        if (inventory == null)
            await throwBusinessException(InventoriesBusinessMessages.InventoryNotExists);
    }

    public async Task InventoryIdShouldExistWhenSelected(Guid id, CancellationToken cancellationToken)
    {
        Inventory? inventory = await _inventoryRepository.GetAsync(
            predicate: i => i.Id == id,
            enableTracking: false,
            cancellationToken: cancellationToken
        );
        await InventoryShouldExistWhenSelected(inventory);
    }
}