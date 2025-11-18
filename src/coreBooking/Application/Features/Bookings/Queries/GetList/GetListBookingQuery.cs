using Application.Features.Bookings.Constants;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using NArchitecture.Core.Application.Pipelines.Authorization;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Paging;
using MediatR;
using static Application.Features.Bookings.Constants.BookingsOperationClaims;

namespace Application.Features.Bookings.Queries.GetList;

public class GetListBookingQuery : IRequest<GetListResponse<GetListBookingListItemDto>>, ISecuredRequest
{
    public PageRequest PageRequest { get; set; }

    public string[] Roles => [Admin, Read];

    public class GetListBookingQueryHandler : IRequestHandler<GetListBookingQuery, GetListResponse<GetListBookingListItemDto>>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IMapper _mapper;

        public GetListBookingQueryHandler(IBookingRepository bookingRepository, IMapper mapper)
        {
            _bookingRepository = bookingRepository;
            _mapper = mapper;
        }

        public async Task<GetListResponse<GetListBookingListItemDto>> Handle(GetListBookingQuery request, CancellationToken cancellationToken)
        {
            IPaginate<Booking> bookings = await _bookingRepository.GetListAsync(
                index: request.PageRequest.PageIndex,
                size: request.PageRequest.PageSize, 
                cancellationToken: cancellationToken
            );

            GetListResponse<GetListBookingListItemDto> response = _mapper.Map<GetListResponse<GetListBookingListItemDto>>(bookings);
            return response;
        }
    }
}