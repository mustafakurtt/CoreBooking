using Application.Features.RoomTypes.Constants;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using NArchitecture.Core.Application.Pipelines.Authorization;
using NArchitecture.Core.Application.Pipelines.Caching;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Paging;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static Application.Features.RoomTypes.Constants.RoomTypesOperationClaims;

namespace Application.Features.RoomTypes.Queries.GetList;

public class GetListRoomTypeQuery : IRequest<GetListResponse<GetListRoomTypeListItemDto>>, ISecuredRequest, ICachableRequest
{
    public PageRequest PageRequest { get; set; }

    public string[] Roles => [Admin, Read];

    public bool BypassCache { get; }
    public string? CacheKey => $"GetListRoomTypes({PageRequest.PageIndex},{PageRequest.PageSize})";
    public string? CacheGroupKey => "GetRoomTypes";
    public TimeSpan? SlidingExpiration { get; }

    public class GetListRoomTypeQueryHandler : IRequestHandler<GetListRoomTypeQuery, GetListResponse<GetListRoomTypeListItemDto>>
    {
        private readonly IRoomTypeRepository _roomTypeRepository;
        private readonly IMapper _mapper;

        public GetListRoomTypeQueryHandler(IRoomTypeRepository roomTypeRepository, IMapper mapper)
        {
            _roomTypeRepository = roomTypeRepository;
            _mapper = mapper;
        }

        public async Task<GetListResponse<GetListRoomTypeListItemDto>> Handle(GetListRoomTypeQuery request, CancellationToken cancellationToken)
        {
            IPaginate<RoomType> roomTypes = await _roomTypeRepository.GetListAsync(
                index: request.PageRequest.PageIndex,
                size: request.PageRequest.PageSize,

                // ÝLÝÞKÝ: Otel bilgisini çek
                include: rt => rt.Include(h => h.Hotel),

                cancellationToken: cancellationToken
            );

            GetListResponse<GetListRoomTypeListItemDto> response = _mapper.Map<GetListResponse<GetListRoomTypeListItemDto>>(roomTypes);
            return response;
        }
    }
}