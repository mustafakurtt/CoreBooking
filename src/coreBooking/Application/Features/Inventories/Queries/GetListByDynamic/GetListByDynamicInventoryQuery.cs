using Application.Features.Hotels.Constants;
using Application.Features.Inventories.Constants;
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

namespace Application.Features.Inventories.Queries.GetListByDynamic;

public class GetListByDynamicInventoryQuery : IRequest<GetListResponse<GetListByDynamicInventoryListItemDto>>, ILoggableRequest, ISecuredRequest
{
    public PageRequest PageRequest { get; set; }
    public DynamicQuery Dynamic { get; set; }

    public string[] Roles => new[] { HotelsOperationClaims.Admin, InventoriesOperationClaims.Read };
        
    

    public class GetListByDynamicInventoryQueryHandler : IRequestHandler<GetListByDynamicInventoryQuery, GetListResponse<GetListByDynamicInventoryListItemDto>>
    {
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IMapper _mapper;

        public GetListByDynamicInventoryQueryHandler(IInventoryRepository inventoryRepository, IMapper mapper)
        {
            _inventoryRepository = inventoryRepository;
            _mapper = mapper;
        }

        public async Task<GetListResponse<GetListByDynamicInventoryListItemDto>> Handle(GetListByDynamicInventoryQuery request, CancellationToken cancellationToken)
        {
            IPaginate<Inventory> inventories = await _inventoryRepository.GetListByDynamicAsync(
                dynamic: request.Dynamic,
                index: request.PageRequest.PageIndex,
                size: request.PageRequest.PageSize,
                cancellationToken: cancellationToken
            );

            GetListResponse<GetListByDynamicInventoryListItemDto> response = _mapper.Map<GetListResponse<GetListByDynamicInventoryListItemDto>>(inventories);
            return response;
        }
    }
}
