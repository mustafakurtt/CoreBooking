using NArchitecture.Core.Persistence.Paging;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Application.Services.Inventories;

public interface IInventoryService
{
    Task<Inventory?> GetAsync(
        Expression<Func<Inventory, bool>> predicate,
        Func<IQueryable<Inventory>, IIncludableQueryable<Inventory, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );
    Task<IPaginate<Inventory>?> GetListAsync(
        Expression<Func<Inventory, bool>>? predicate = null,
        Func<IQueryable<Inventory>, IOrderedQueryable<Inventory>>? orderBy = null,
        Func<IQueryable<Inventory>, IIncludableQueryable<Inventory, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );
    Task<Inventory> AddAsync(Inventory inventory);
    Task<Inventory> UpdateAsync(Inventory inventory);
    Task<Inventory> DeleteAsync(Inventory inventory, bool permanent = false);
}
