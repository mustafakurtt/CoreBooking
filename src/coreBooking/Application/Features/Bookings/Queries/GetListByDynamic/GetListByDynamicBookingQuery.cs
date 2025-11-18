using Application.Features.Bookings.Constants;
using Application.Features.Hotels.Constants;
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

namespace Application.Features.Bookings.Queries.GetListByDynamic;

public class GetListByDynamicBookingQuery : IRequest<GetListResponse<GetListByDynamicBookingListItemDto>>, ILoggableRequest, ISecuredRequest
{
    public PageRequest PageRequest { get; set; }
    public DynamicQuery Dynamic { get; set; }

    public string[] Roles => new[] { HotelsOperationClaims.Admin, BookingsOperationClaims.Read };
        
    

    public class GetListByDynamicBookingQueryHandler : IRequestHandler<GetListByDynamicBookingQuery, GetListResponse<GetListByDynamicBookingListItemDto>>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IMapper _mapper;

        public GetListByDynamicBookingQueryHandler(IBookingRepository bookingRepository, IMapper mapper)
        {
            _bookingRepository = bookingRepository;
            _mapper = mapper;
        }

        public async Task<GetListResponse<GetListByDynamicBookingListItemDto>> Handle(GetListByDynamicBookingQuery request, CancellationToken cancellationToken)
        {
            IPaginate<Booking> bookings = await _bookingRepository.GetListByDynamicAsync(
                dynamic: request.Dynamic,
                index: request.PageRequest.PageIndex,
                size: request.PageRequest.PageSize,
                cancellationToken: cancellationToken
            );

            GetListResponse<GetListByDynamicBookingListItemDto> response = _mapper.Map<GetListResponse<GetListByDynamicBookingListItemDto>>(bookings);
            return response;
        }
    }
}
