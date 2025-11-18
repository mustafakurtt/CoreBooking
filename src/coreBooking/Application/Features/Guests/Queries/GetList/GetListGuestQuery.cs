using Application.Features.Guests.Constants;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using NArchitecture.Core.Application.Pipelines.Authorization;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Paging;
using MediatR;
using static Application.Features.Guests.Constants.GuestsOperationClaims;

namespace Application.Features.Guests.Queries.GetList;

public class GetListGuestQuery : IRequest<GetListResponse<GetListGuestListItemDto>>, ISecuredRequest
{
    public PageRequest PageRequest { get; set; }

    public string[] Roles => [Admin, Read];

    public class GetListGuestQueryHandler : IRequestHandler<GetListGuestQuery, GetListResponse<GetListGuestListItemDto>>
    {
        private readonly IGuestRepository _guestRepository;
        private readonly IMapper _mapper;

        public GetListGuestQueryHandler(IGuestRepository guestRepository, IMapper mapper)
        {
            _guestRepository = guestRepository;
            _mapper = mapper;
        }

        public async Task<GetListResponse<GetListGuestListItemDto>> Handle(GetListGuestQuery request, CancellationToken cancellationToken)
        {
            IPaginate<Guest> guests = await _guestRepository.GetListAsync(
                index: request.PageRequest.PageIndex,
                size: request.PageRequest.PageSize, 
                cancellationToken: cancellationToken
            );

            GetListResponse<GetListGuestListItemDto> response = _mapper.Map<GetListResponse<GetListGuestListItemDto>>(guests);
            return response;
        }
    }
}