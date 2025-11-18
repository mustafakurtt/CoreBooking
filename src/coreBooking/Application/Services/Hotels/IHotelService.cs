using NArchitecture.Core.Persistence.Paging;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Application.Services.Hotels;

public interface IHotelService
{
    Task<Hotel?> GetAsync(
        Expression<Func<Hotel, bool>> predicate,
        Func<IQueryable<Hotel>, IIncludableQueryable<Hotel, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );
    Task<IPaginate<Hotel>?> GetListAsync(
        Expression<Func<Hotel, bool>>? predicate = null,
        Func<IQueryable<Hotel>, IOrderedQueryable<Hotel>>? orderBy = null,
        Func<IQueryable<Hotel>, IIncludableQueryable<Hotel, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );
    Task<Hotel> AddAsync(Hotel hotel);
    Task<Hotel> UpdateAsync(Hotel hotel);
    Task<Hotel> DeleteAsync(Hotel hotel, bool permanent = false);
}
