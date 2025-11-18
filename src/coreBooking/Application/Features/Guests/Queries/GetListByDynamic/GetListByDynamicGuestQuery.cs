using Application.Features.Guests.Constants;
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

namespace Application.Features.Guests.Queries.GetListByDynamic;

public class GetListByDynamicGuestQuery : IRequest<GetListResponse<GetListByDynamicGuestListItemDto>>, ILoggableRequest, ISecuredRequest
{
    public PageRequest PageRequest { get; set; }
    public DynamicQuery Dynamic { get; set; }

    public string[] Roles => new[] { GuestsOperationClaims.Admin, GuestsOperationClaims.Read };
        
    

    public class GetListByDynamicGuestQueryHandler : IRequestHandler<GetListByDynamicGuestQuery, GetListResponse<GetListByDynamicGuestListItemDto>>
    {
        private readonly IGuestRepository _guestRepository;
        private readonly IMapper _mapper;

        public GetListByDynamicGuestQueryHandler(IGuestRepository guestRepository, IMapper mapper)
        {
            _guestRepository = guestRepository;
            _mapper = mapper;
        }

        public async Task<GetListResponse<GetListByDynamicGuestListItemDto>> Handle(GetListByDynamicGuestQuery request, CancellationToken cancellationToken)
        {
            IPaginate<Guest> guests = await _guestRepository.GetListByDynamicAsync(
                dynamic: request.Dynamic,
                index: request.PageRequest.PageIndex,
                size: request.PageRequest.PageSize,
                cancellationToken: cancellationToken
            );

            GetListResponse<GetListByDynamicGuestListItemDto> response = _mapper.Map<GetListResponse<GetListByDynamicGuestListItemDto>>(guests);
            return response;
        }
    }
}
