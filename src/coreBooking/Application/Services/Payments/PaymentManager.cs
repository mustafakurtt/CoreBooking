using Application.Features.Payments.Rules;
using Application.Services.Repositories;
using NArchitecture.Core.Persistence.Paging;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Application.Services.Payments;

public class PaymentManager : IPaymentService
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly PaymentBusinessRules _paymentBusinessRules;

    public PaymentManager(IPaymentRepository paymentRepository, PaymentBusinessRules paymentBusinessRules)
    {
        _paymentRepository = paymentRepository;
        _paymentBusinessRules = paymentBusinessRules;
    }

    public async Task<Payment?> GetAsync(
        Expression<Func<Payment, bool>> predicate,
        Func<IQueryable<Payment>, IIncludableQueryable<Payment, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    )
    {
        Payment? payment = await _paymentRepository.GetAsync(predicate, include, withDeleted, enableTracking, cancellationToken);
        return payment;
    }

    public async Task<IPaginate<Payment>?> GetListAsync(
        Expression<Func<Payment, bool>>? predicate = null,
        Func<IQueryable<Payment>, IOrderedQueryable<Payment>>? orderBy = null,
        Func<IQueryable<Payment>, IIncludableQueryable<Payment, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    )
    {
        IPaginate<Payment> paymentList = await _paymentRepository.GetListAsync(
            predicate,
            orderBy,
            include,
            index,
            size,
            withDeleted,
            enableTracking,
            cancellationToken
        );
        return paymentList;
    }

    public async Task<Payment> AddAsync(Payment payment)
    {
        Payment addedPayment = await _paymentRepository.AddAsync(payment);

        return addedPayment;
    }

    public async Task<Payment> UpdateAsync(Payment payment)
    {
        Payment updatedPayment = await _paymentRepository.UpdateAsync(payment);

        return updatedPayment;
    }

    public async Task<Payment> DeleteAsync(Payment payment, bool permanent = false)
    {
        Payment deletedPayment = await _paymentRepository.DeleteAsync(payment);

        return deletedPayment;
    }
}
