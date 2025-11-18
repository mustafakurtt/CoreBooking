using NArchitecture.Core.Persistence.Paging;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Application.Services.RoomTypes;

public interface IRoomTypeService
{
    Task<RoomType?> GetAsync(
        Expression<Func<RoomType, bool>> predicate,
        Func<IQueryable<RoomType>, IIncludableQueryable<RoomType, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );
    Task<IPaginate<RoomType>?> GetListAsync(
        Expression<Func<RoomType, bool>>? predicate = null,
        Func<IQueryable<RoomType>, IOrderedQueryable<RoomType>>? orderBy = null,
        Func<IQueryable<RoomType>, IIncludableQueryable<RoomType, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );
    Task<RoomType> AddAsync(RoomType roomType);
    Task<RoomType> UpdateAsync(RoomType roomType);
    Task<RoomType> DeleteAsync(RoomType roomType, bool permanent = false);
}
