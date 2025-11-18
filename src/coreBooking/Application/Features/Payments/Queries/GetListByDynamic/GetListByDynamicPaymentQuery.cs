using Application.Features.Payments.Constants;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NArchitecture.Core.Application.Pipelines.Authorization;
using NArchitecture.Core.Application.Pipelines.Caching;
using NArchitecture.Core.Application.Pipelines.Logging;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Dynamic;
using NArchitecture.Core.Persistence.Paging;

namespace Application.Features.Payments.Queries.GetListByDynamic;

public class GetListByDynamicPaymentQuery : IRequest<GetListResponse<GetListByDynamicPaymentListItemDto>>, ILoggableRequest, ISecuredRequest
{
    public PageRequest PageRequest { get; set; }
    public DynamicQuery Dynamic { get; set; }

    public string[] Roles => [PaymentsOperationClaims.Admin, PaymentsOperationClaims.Read];

    public class GetListByDynamicPaymentQueryHandler : IRequestHandler<GetListByDynamicPaymentQuery, GetListResponse<GetListByDynamicPaymentListItemDto>>
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IMapper _mapper;

        public GetListByDynamicPaymentQueryHandler(IPaymentRepository paymentRepository, IMapper mapper)
        {
            _paymentRepository = paymentRepository;
            _mapper = mapper;
        }

        public async Task<GetListResponse<GetListByDynamicPaymentListItemDto>> Handle(GetListByDynamicPaymentQuery request, CancellationToken cancellationToken)
        {
            IPaginate<Payment> payments = await _paymentRepository.GetListByDynamicAsync(
                dynamic: request.Dynamic,
                index: request.PageRequest.PageIndex,
                size: request.PageRequest.PageSize,
                cancellationToken: cancellationToken
            );

            GetListResponse<GetListByDynamicPaymentListItemDto> response = _mapper.Map<GetListResponse<GetListByDynamicPaymentListItemDto>>(payments);
            return response;
        }
    }
}