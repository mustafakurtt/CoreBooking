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

namespace Application.Features.Payments.Commands.Create;

public class CreatePaymentCommand : IRequest<CreatedPaymentResponse>, ISecuredRequest, ILoggableRequest, ITransactionalRequest
{
    public required Guid BookingId { get; set; }

    // --- V2: Flat Input (Value Object yerine düz veri alýyoruz) ---
    public required decimal AmountValue { get; set; }
    public required Currency AmountCurrency { get; set; }

    public required DateTime Date { get; set; }
    public required string TransactionId { get; set; }
    public required PaymentStatus Status { get; set; }

    public string[] Roles => [Admin, Write, PaymentsOperationClaims.Create];

    public class CreatePaymentCommandHandler : IRequestHandler<CreatePaymentCommand, CreatedPaymentResponse>
    {
        private readonly IMapper _mapper;
        private readonly IPaymentRepository _paymentRepository;
        private readonly PaymentBusinessRules _paymentBusinessRules;

        public CreatePaymentCommandHandler(IMapper mapper, IPaymentRepository paymentRepository,
                                           PaymentBusinessRules paymentBusinessRules)
        {
            _mapper = mapper;
            _paymentRepository = paymentRepository;
            _paymentBusinessRules = paymentBusinessRules;
        }

        public async Task<CreatedPaymentResponse> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
        {
            // 1. ÝÞ KURALLARI (Business Rules)
            // Önce rezervasyon var mý diye bakýyoruz
            await _paymentBusinessRules.BookingIdShouldExist(request.BookingId, cancellationToken);

            // Ayný iþlem numarasýyla (örn: Banka Ref No) daha önce kayýt atýlmýþ mý?
            await _paymentBusinessRules.TransactionIdShouldNotExists(request.TransactionId, cancellationToken);

            // 2. DOMAIN LOGIC (Value Object Oluþturma)
            Money money = new(request.AmountValue, request.AmountCurrency);

            // 3. ENTITY OLUÞTURMA (Manuel Mapping)
            Payment payment = new()
            {
                Id = Guid.NewGuid(),
                BookingId = request.BookingId,
                Amount = money, // Value Object atamasý
                Date = request.Date,
                TransactionId = request.TransactionId,
                Status = request.Status
            };

            // 4. VERÝTABANI KAYIT
            await _paymentRepository.AddAsync(payment);

            // 5. RESPONSE
            CreatedPaymentResponse response = _mapper.Map<CreatedPaymentResponse>(payment);
            return response;
        }
    }
}