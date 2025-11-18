using Application.Features.Inventories.Rules;
using Application.Services.Repositories;
using NArchitecture.Core.Persistence.Paging;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Application.Services.Inventories;

public class InventoryManager : IInventoryService
{
    private readonly IInventoryRepository _inventoryRepository;
    private readonly InventoryBusinessRules _inventoryBusinessRules;

    public InventoryManager(IInventoryRepository inventoryRepository, InventoryBusinessRules inventoryBusinessRules)
    {
        _inventoryRepository = inventoryRepository;
        _inventoryBusinessRules = inventoryBusinessRules;
    }

    public async Task<Inventory?> GetAsync(
        Expression<Func<Inventory, bool>> predicate,
        Func<IQueryable<Inventory>, IIncludableQueryable<Inventory, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    )
    {
        Inventory? inventory = await _inventoryRepository.GetAsync(predicate, include, withDeleted, enableTracking, cancellationToken);
        return inventory;
    }

    public async Task<IPaginate<Inventory>?> GetListAsync(
        Expression<Func<Inventory, bool>>? predicate = null,
        Func<IQueryable<Inventory>, IOrderedQueryable<Inventory>>? orderBy = null,
        Func<IQueryable<Inventory>, IIncludableQueryable<Inventory, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    )
    {
        IPaginate<Inventory> inventoryList = await _inventoryRepository.GetListAsync(
            predicate,
            orderBy,
            include,
            index,
            size,
            withDeleted,
            enableTracking,
            cancellationToken
        );
        return inventoryList;
    }

    public async Task<Inventory> AddAsync(Inventory inventory)
    {
        Inventory addedInventory = await _inventoryRepository.AddAsync(inventory);

        return addedInventory;
    }

    public async Task<Inventory> UpdateAsync(Inventory inventory)
    {
        Inventory updatedInventory = await _inventoryRepository.UpdateAsync(inventory);

        return updatedInventory;
    }

    public async Task<Inventory> DeleteAsync(Inventory inventory, bool permanent = false)
    {
        Inventory deletedInventory = await _inventoryRepository.DeleteAsync(inventory);

        return deletedInventory;
    }
}
