using Application.Features.Inventories.Constants;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using NArchitecture.Core.Application.Pipelines.Authorization;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Paging;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static Application.Features.Inventories.Constants.InventoriesOperationClaims;

namespace Application.Features.Inventories.Queries.GetList;

public class GetListInventoryQuery : IRequest<GetListResponse<GetListInventoryListItemDto>>, ISecuredRequest
{
    public PageRequest PageRequest { get; set; }

    public string[] Roles => [Admin, Read];

    public class GetListInventoryQueryHandler : IRequestHandler<GetListInventoryQuery, GetListResponse<GetListInventoryListItemDto>>
    {
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IMapper _mapper;

        public GetListInventoryQueryHandler(IInventoryRepository inventoryRepository, IMapper mapper)
        {
            _inventoryRepository = inventoryRepository;
            _mapper = mapper;
        }

        public async Task<GetListResponse<GetListInventoryListItemDto>> Handle(GetListInventoryQuery request, CancellationToken cancellationToken)
        {
            IPaginate<Inventory> inventories = await _inventoryRepository.GetListAsync(
                index: request.PageRequest.PageIndex,
                size: request.PageRequest.PageSize,

                // 🌟 ZİNCİRLEME İLİŞKİLERİ YÜKLÜYORUZ (Inventory -> RoomType -> Hotel)
                include: i => i.Include(x => x.RoomType)
                    .ThenInclude(rt => rt.Hotel),

                cancellationToken: cancellationToken
            );

            GetListResponse<GetListInventoryListItemDto> response = _mapper.Map<GetListResponse<GetListInventoryListItemDto>>(inventories);
            return response;
        }
    }
}