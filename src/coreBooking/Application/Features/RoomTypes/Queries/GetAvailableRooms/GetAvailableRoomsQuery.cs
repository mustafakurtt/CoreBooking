using Application.Features.RoomTypes.Constants;
using Application.Services.Repositories;
using AutoMapper;
using MediatR;
using NArchitecture.Core.Application.Pipelines.Authorization;
using NArchitecture.Core.Application.Pipelines.Caching;
using NArchitecture.Core.Application.Pipelines.Logging;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Dynamic;
using NArchitecture.Core.Persistence.Paging;

namespace Application.Features.RoomTypes.Queries.GetAvailableRooms;

public class GetAvailableRoomsQuery : IRequest<GetListResponse<GetAvailableRoomsResponse>>, ISecuredRequest, ILoggableRequest, ICachableRequest
{
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
    public int NumberOfGuests { get; set; }
    public PageRequest PageRequest { get; set; }
    public DynamicQuery? DynamicQuery { get; set; }

    public string[] Roles => [RoomTypesOperationClaims.Admin, RoomTypesOperationClaims.Read];


    public bool BypassCache { get; set; }
    public string CacheKey => $"GetAvailableRooms-{CheckInDate:yyyyMMdd}-{CheckOutDate:yyyyMMdd}-{NumberOfGuests}-{PageRequest.PageIndex}-{PageRequest.PageSize}";
    public string? CacheGroupKey => "GetAvailableRooms";
    public TimeSpan? SlidingExpiration => TimeSpan.FromMinutes(5);

    public GetAvailableRoomsQuery()
    {
        PageRequest = new PageRequest { PageIndex = 0, PageSize = 10 };
    }

    public class GetAvailableRoomsQueryHandler : IRequestHandler<GetAvailableRoomsQuery, GetListResponse<GetAvailableRoomsResponse>>
    {
        private readonly IRoomTypeRepository _roomTypeRepository;
        private readonly IMapper _mapper;

        public GetAvailableRoomsQueryHandler(IRoomTypeRepository roomTypeRepository, IMapper mapper)
        {
            _roomTypeRepository = roomTypeRepository;
            _mapper = mapper;
        }

        public async Task<GetListResponse<GetAvailableRoomsResponse>> Handle(GetAvailableRoomsQuery request, CancellationToken cancellationToken)
        {
            IPaginate<GetAvailableRoomsResponse> availableRooms = await _roomTypeRepository.GetAvailableRoomsAsync(
                checkInDate: request.CheckInDate,
                checkOutDate: request.CheckOutDate,
                numberOfGuests: request.NumberOfGuests,
                pageRequest: request.PageRequest,
                dynamicQuery: request.DynamicQuery,
                cancellationToken: cancellationToken
            );

            GetListResponse<GetAvailableRoomsResponse> response = _mapper.Map<GetListResponse<GetAvailableRoomsResponse>>(availableRooms);

            return response;
        }
    }
}