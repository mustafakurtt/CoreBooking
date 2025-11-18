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

namespace Application.Features.Hotels.Queries.GetListByDynamic;

public class GetListByDynamicHotelQuery : IRequest<GetListResponse<GetListByDynamicHotelListItemDto>>, ISecuredRequest, ILoggableRequest
{
    public PageRequest PageRequest { get; set; }
    public DynamicQuery Dynamic { get; set; }

    public string[] Roles => [HotelsOperationClaims.Admin, HotelsOperationClaims.Read];

    public class GetListByDynamicHotelQueryHandler : IRequestHandler<GetListByDynamicHotelQuery, GetListResponse<GetListByDynamicHotelListItemDto>>
    {
        private readonly IHotelRepository _hotelRepository;
        private readonly IMapper _mapper;

        public GetListByDynamicHotelQueryHandler(IHotelRepository hotelRepository, IMapper mapper)
        {
            _hotelRepository = hotelRepository;
            _mapper = mapper;
        }

        public async Task<GetListResponse<GetListByDynamicHotelListItemDto>> Handle(GetListByDynamicHotelQuery request, CancellationToken cancellationToken)
        {
            IPaginate<Hotel> hotels = await _hotelRepository.GetListByDynamicAsync(
                dynamic: request.Dynamic,
                index: request.PageRequest.PageIndex,
                size: request.PageRequest.PageSize,

                // 🌟 İLİŞKİLERİ DAHİL ET (RoomTypeCount için gerekli)
                include: h => h.Include(x => x.RoomTypes),

                cancellationToken: cancellationToken
            );

            GetListResponse<GetListByDynamicHotelListItemDto> response = _mapper.Map<GetListResponse<GetListByDynamicHotelListItemDto>>(hotels);
            return response;
        }
    }
}
