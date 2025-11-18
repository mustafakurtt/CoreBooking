using Application.Features.Payments.Constants;
using Application.Features.Payments.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using NArchitecture.Core.Application.Pipelines.Authorization;
using NArchitecture.Core.Application.Pipelines.Logging;
using NArchitecture.Core.Application.Pipelines.Transaction;
using MediatR;
using Domain.ValueObjects;
using Shared.Enums;
using static Application.Features.Payments.Constants.PaymentsOperationClaims;

namespace Application.Features.Payments.Commands.Update;

public class UpdatePaymentCommand : IRequest<UpdatedPaymentResponse>, ISecuredRequest, ILoggableRequest, ITransactionalRequest
{
    public Guid Id { get; set; }
    public required Guid BookingId { get; set; }

    // --- V2: Flat Input ---
    public required decimal AmountValue { get; set; }
    public required Currency AmountCurrency { get; set; }

    public required DateTime Date { get; set; }
    public required string TransactionId { get; set; }
    public required PaymentStatus Status { get; set; }

    public string[] Roles => [Admin, Write, PaymentsOperationClaims.Update];

    public class UpdatePaymentCommandHandler : IRequestHandler<UpdatePaymentCommand, UpdatedPaymentResponse>
    {
        private readonly IMapper _mapper;
        private readonly IPaymentRepository _paymentRepository;
        private readonly PaymentBusinessRules _paymentBusinessRules;

        public UpdatePaymentCommandHandler(IMapper mapper, IPaymentRepository paymentRepository,
                                           PaymentBusinessRules paymentBusinessRules)
        {
            _mapper = mapper;
            _paymentRepository = paymentRepository;
            _paymentBusinessRules = paymentBusinessRules;
        }

        public async Task<UpdatedPaymentResponse> Handle(UpdatePaymentCommand request, CancellationToken cancellationToken)
        {
            // 1. Kaydý Getir
            Payment? payment = await _paymentRepository.GetAsync(
                predicate: p => p.Id == request.Id,
                cancellationToken: cancellationToken
            );

            // 2. Kurallarý Çalýþtýr
            await _paymentBusinessRules.PaymentShouldExistWhenSelected(payment);
            await _paymentBusinessRules.BookingIdShouldExist(request.BookingId, cancellationToken);

            // TransactionId deðiþtiyse benzersizlik kontrolü yap
            if (payment!.TransactionId != request.TransactionId)
            {
                await _paymentBusinessRules.TransactionIdShouldNotExistsWhenUpdate(request.Id, request.TransactionId, cancellationToken);
            }

            // 3. Mapping (AutoMapper veya Manuel)
            // Burada Value Object'i elle oluþturup atamak en güvenlisidir.
            // AutoMapper bazen "AmountValue -> Money" dönüþümünü kaçýrabilir.
            _mapper.Map(request, payment); // Basit alanlarý (Date, Status vb.) maple

            // Value Object'i manuel set ediyoruz
            payment.Amount = new Money(request.AmountValue, request.AmountCurrency);

            // 4. Güncelle
            await _paymentRepository.UpdateAsync(payment!);

            // 5. Response Dön
            UpdatedPaymentResponse response = _mapper.Map<UpdatedPaymentResponse>(payment);
            return response;
        }
    }
}