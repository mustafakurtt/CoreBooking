using NArchitecture.Core.Persistence.Paging;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Application.Services.Guests;

public interface IGuestService
{
    Task<Guest?> GetAsync(
        Expression<Func<Guest, bool>> predicate,
        Func<IQueryable<Guest>, IIncludableQueryable<Guest, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );
    Task<IPaginate<Guest>?> GetListAsync(
        Expression<Func<Guest, bool>>? predicate = null,
        Func<IQueryable<Guest>, IOrderedQueryable<Guest>>? orderBy = null,
        Func<IQueryable<Guest>, IIncludableQueryable<Guest, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );
    Task<Guest> AddAsync(Guest guest);
    Task<Guest> UpdateAsync(Guest guest);
    Task<Guest> DeleteAsync(Guest guest, bool permanent = false);
}
