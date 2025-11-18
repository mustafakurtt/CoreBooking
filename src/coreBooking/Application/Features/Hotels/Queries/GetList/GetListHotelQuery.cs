using Application.Features.Hotels.Constants;
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
using static Application.Features.Hotels.Constants.HotelsOperationClaims;

namespace Application.Features.Hotels.Queries.GetList;

public class GetListHotelQuery : IRequest<GetListResponse<GetListHotelListItemDto>>, ISecuredRequest, ICachableRequest
{
    public PageRequest PageRequest { get; set; }

    public string[] Roles => [Admin, Read];

    public bool BypassCache { get; }
    public string? CacheKey => $"GetListHotels({PageRequest.PageIndex},{PageRequest.PageSize})";
    public string? CacheGroupKey => "GetHotels";
    public TimeSpan? SlidingExpiration { get; }

    public class GetListHotelQueryHandler : IRequestHandler<GetListHotelQuery, GetListResponse<GetListHotelListItemDto>>
    {
        private readonly IHotelRepository _hotelRepository;
        private readonly IMapper _mapper;

        public GetListHotelQueryHandler(IHotelRepository hotelRepository, IMapper mapper)
        {
            _hotelRepository = hotelRepository;
            _mapper = mapper;
        }

        public async Task<GetListResponse<GetListHotelListItemDto>> Handle(GetListHotelQuery request, CancellationToken cancellationToken)
        {
            IPaginate<Hotel> hotels = await _hotelRepository.GetListAsync(
                index: request.PageRequest.PageIndex,
                size: request.PageRequest.PageSize,

                // Ýliþkiyi yüklüyoruz ki sayýsýný alabilelim
                include: h => h.Include(x => x.RoomTypes),

                cancellationToken: cancellationToken
            );

            GetListResponse<GetListHotelListItemDto> response = _mapper.Map<GetListResponse<GetListHotelListItemDto>>(hotels);
            return response;
        }
    }
}