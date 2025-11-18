using Application.Features.Payments.Constants;
using Application.Features.Payments.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using NArchitecture.Core.Application.Pipelines.Authorization;
using MediatR;
using static Application.Features.Payments.Constants.PaymentsOperationClaims;

namespace Application.Features.Payments.Queries.GetById;

public class GetByIdPaymentQuery : IRequest<GetByIdPaymentResponse>, ISecuredRequest
{
    public Guid Id { get; set; }

    public string[] Roles => [Admin, Read];

    public class GetByIdPaymentQueryHandler : IRequestHandler<GetByIdPaymentQuery, GetByIdPaymentResponse>
    {
        private readonly IMapper _mapper;
        private readonly IPaymentRepository _paymentRepository;
        private readonly PaymentBusinessRules _paymentBusinessRules;

        public GetByIdPaymentQueryHandler(IMapper mapper, IPaymentRepository paymentRepository, PaymentBusinessRules paymentBusinessRules)
        {
            _mapper = mapper;
            _paymentRepository = paymentRepository;
            _paymentBusinessRules = paymentBusinessRules;
        }

        public async Task<GetByIdPaymentResponse> Handle(GetByIdPaymentQuery request, CancellationToken cancellationToken)
        {
            // Repository altyapýmýz "Include" desteði sunar.
            // Ýleride Booking veya User bilgilerini de çekmek isterseniz buraya .Include() ekleyebilirsiniz.
            Payment? payment = await _paymentRepository.GetAsync(
                predicate: p => p.Id == request.Id,
                // include: p => p.Include(x => x.Booking), // Örnek Include kullanýmý
                cancellationToken: cancellationToken
            );

            await _paymentBusinessRules.PaymentShouldExistWhenSelected(payment);

            GetByIdPaymentResponse response = _mapper.Map<GetByIdPaymentResponse>(payment);
            return response;
        }
    }
}