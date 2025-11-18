using NArchitecture.Core.Persistence.Paging;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Application.Services.Bookings;

public interface IBookingService
{
    Task<Booking?> GetAsync(
        Expression<Func<Booking, bool>> predicate,
        Func<IQueryable<Booking>, IIncludableQueryable<Booking, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );
    Task<IPaginate<Booking>?> GetListAsync(
        Expression<Func<Booking, bool>>? predicate = null,
        Func<IQueryable<Booking>, IOrderedQueryable<Booking>>? orderBy = null,
        Func<IQueryable<Booking>, IIncludableQueryable<Booking, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );
    Task<Booking> AddAsync(Booking booking);
    Task<Booking> UpdateAsync(Booking booking);
    Task<Booking> DeleteAsync(Booking booking, bool permanent = false);
}
