using Application.Features.Payments.Constants;
using Application.Features.Payments.Constants;
using Application.Features.Payments.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using NArchitecture.Core.Application.Pipelines.Authorization;
using NArchitecture.Core.Application.Pipelines.Logging;
using NArchitecture.Core.Application.Pipelines.Transaction;
using MediatR;
using static Application.Features.Payments.Constants.PaymentsOperationClaims;

namespace Application.Features.Payments.Commands.Delete;

public class DeletePaymentCommand : IRequest<DeletedPaymentResponse>, ISecuredRequest, ILoggableRequest, ITransactionalRequest
{
    public Guid Id { get; set; }

    public string[] Roles => [Admin, Write, PaymentsOperationClaims.Delete];

    public class DeletePaymentCommandHandler : IRequestHandler<DeletePaymentCommand, DeletedPaymentResponse>
    {
        private readonly IMapper _mapper;
        private readonly IPaymentRepository _paymentRepository;
        private readonly PaymentBusinessRules _paymentBusinessRules;

        public DeletePaymentCommandHandler(IMapper mapper, IPaymentRepository paymentRepository,
            PaymentBusinessRules paymentBusinessRules)
        {
            _mapper = mapper;
            _paymentRepository = paymentRepository;
            _paymentBusinessRules = paymentBusinessRules;
        }

        public async Task<DeletedPaymentResponse> Handle(DeletePaymentCommand request, CancellationToken cancellationToken)
        {
            // 1. Kaydý Getir
            Payment? payment = await _paymentRepository.GetAsync(
                predicate: p => p.Id == request.Id,
                cancellationToken: cancellationToken
            );

            // 2. Varlýk Kontrolü
            await _paymentBusinessRules.PaymentShouldExistWhenSelected(payment);

            // 3. YENÝ: Silinebilirlik Kontrolü (Finansal Güvenlik)
            await _paymentBusinessRules.PaymentShouldNotBeCompletedOrRefunded(payment!);

            // 4. Sil (Soft Delete)
            await _paymentRepository.DeleteAsync(payment!);

            // 5. Response
            DeletedPaymentResponse response = _mapper.Map<DeletedPaymentResponse>(payment);
            return response;
        }
    }
}