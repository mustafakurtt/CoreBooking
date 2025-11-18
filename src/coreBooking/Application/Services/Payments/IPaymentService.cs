using NArchitecture.Core.Persistence.Paging;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Application.Services.Payments;

public interface IPaymentService
{
    Task<Payment?> GetAsync(
        Expression<Func<Payment, bool>> predicate,
        Func<IQueryable<Payment>, IIncludableQueryable<Payment, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );
    Task<IPaginate<Payment>?> GetListAsync(
        Expression<Func<Payment, bool>>? predicate = null,
        Func<IQueryable<Payment>, IOrderedQueryable<Payment>>? orderBy = null,
        Func<IQueryable<Payment>, IIncludableQueryable<Payment, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );
    Task<Payment> AddAsync(Payment payment);
    Task<Payment> UpdateAsync(Payment payment);
    Task<Payment> DeleteAsync(Payment payment, bool permanent = false);
}
