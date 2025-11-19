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
    private readonly IRoomTypeRepository _roomTypeRepository; // EKLENDÝ: Oda tipi kontrolü için
    private readonly ILocalizationService _localizationService;

    public InventoryBusinessRules(
        IInventoryRepository inventoryRepository,
        IRoomTypeRepository roomTypeRepository, // EKLENDÝ
        ILocalizationService localizationService)
    {
        _inventoryRepository = inventoryRepository;
        _roomTypeRepository = roomTypeRepository;
        _localizationService = localizationService;
    }

    private async Task throwBusinessException(string messageKey)
    {
        string message = await _localizationService.GetLocalizedAsync(messageKey, InventoriesBusinessMessages.SectionName);
        throw new BusinessException(message);
    }

    // --- 1. VARLIK KONTROLLERÝ ---

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
    public async Task RoomTypeIdShouldExist(Guid roomTypeId, CancellationToken cancellationToken)
    {
        bool doesExist = await _roomTypeRepository.AnyAsync(
            predicate: rt => rt.Id == roomTypeId,
            cancellationToken: cancellationToken
        );

        if (!doesExist)
            await throwBusinessException(InventoriesBusinessMessages.RoomTypeNotExists);
    }
    public async Task InventoryShouldNotExistsWhenInsert(Guid roomTypeId, DateTime date, CancellationToken cancellationToken)
    {
        bool doesExist = await _inventoryRepository.AnyAsync(
            predicate: i => i.RoomTypeId == roomTypeId && i.Date.Date == date.Date,
            cancellationToken: cancellationToken
        );

        if (doesExist)
            await throwBusinessException(InventoriesBusinessMessages.InventoryAlreadyExists);
    }

    public async Task InventoryShouldNotExistsWhenUpdate(Guid id, Guid roomTypeId, DateTime date, CancellationToken cancellationToken)
    {
        // Kendisi hariç (i.Id != id) ayný oda ve tarihte kayýt var mý?
        bool doesExist = await _inventoryRepository.AnyAsync(
            predicate: i => i.Id != id && i.RoomTypeId == roomTypeId && i.Date.Date == date.Date,
            cancellationToken: cancellationToken
        );

        if (doesExist)
            await throwBusinessException(InventoriesBusinessMessages.InventoryAlreadyExists);
    }

    // Kural: Geçmiþe dönük stok iþlemi yapýlamaz (Veri tutarlýlýðý)
    public async Task InventoryDateShouldNotBeInPast(DateTime date)
    {
        if (date.Date < DateTime.Today)
        {
            await throwBusinessException(InventoriesBusinessMessages.InventoryDateCannotBeInPast);
        }
    }
}