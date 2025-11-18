using Application.Features.Hotels.Constants;
using Application.Features.RoomTypes.Constants;
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

namespace Application.Features.RoomTypes.Queries.GetListByDynamic;

public class GetListByDynamicRoomTypeQuery : IRequest<GetListResponse<GetListByDynamicRoomTypeListItemDto>>, ICachableRequest, ILoggableRequest, ISecuredRequest
{
    public PageRequest PageRequest { get; set; }
    public DynamicQuery Dynamic { get; set; }

    public string[] Roles => new[] { HotelsOperationClaims.Admin, RoomTypesOperationClaims.Read };
        
    public bool BypassCache { get; }
        public string CacheKey => $"GetListByDynamicRoomType-{PageRequest.PageIndex}-{PageRequest.PageSize}";
        public string CacheGroupKey => "GetRoomTypes";
        public TimeSpan? SlidingExpiration { get; }
        

    public class GetListByDynamicRoomTypeQueryHandler : IRequestHandler<GetListByDynamicRoomTypeQuery, GetListResponse<GetListByDynamicRoomTypeListItemDto>>
    {
        private readonly IRoomTypeRepository _roomTypeRepository;
        private readonly IMapper _mapper;

        public GetListByDynamicRoomTypeQueryHandler(IRoomTypeRepository roomTypeRepository, IMapper mapper)
        {
            _roomTypeRepository = roomTypeRepository;
            _mapper = mapper;
        }

        public async Task<GetListResponse<GetListByDynamicRoomTypeListItemDto>> Handle(GetListByDynamicRoomTypeQuery request, CancellationToken cancellationToken)
        {
            IPaginate<RoomType> roomTypes = await _roomTypeRepository.GetListByDynamicAsync(
                dynamic: request.Dynamic,
                index: request.PageRequest.PageIndex,
                size: request.PageRequest.PageSize,
                cancellationToken: cancellationToken
            );

            GetListResponse<GetListByDynamicRoomTypeListItemDto> response = _mapper.Map<GetListResponse<GetListByDynamicRoomTypeListItemDto>>(roomTypes);
            return response;
        }
    }
}
