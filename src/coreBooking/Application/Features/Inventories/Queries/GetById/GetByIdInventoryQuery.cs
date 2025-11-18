using Application.Features.Inventories.Constants;
using Application.Features.Inventories.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using NArchitecture.Core.Application.Pipelines.Authorization;
using MediatR;
using static Application.Features.Inventories.Constants.InventoriesOperationClaims;

namespace Application.Features.Inventories.Queries.GetById;

public class GetByIdInventoryQuery : IRequest<GetByIdInventoryResponse>, ISecuredRequest
{
    public Guid Id { get; set; }

    public string[] Roles => [Admin, Read];

    public class GetByIdInventoryQueryHandler : IRequestHandler<GetByIdInventoryQuery, GetByIdInventoryResponse>
    {
        private readonly IMapper _mapper;
        private readonly IInventoryRepository _inventoryRepository;
        private readonly InventoryBusinessRules _inventoryBusinessRules;

        public GetByIdInventoryQueryHandler(IMapper mapper, IInventoryRepository inventoryRepository, InventoryBusinessRules inventoryBusinessRules)
        {
            _mapper = mapper;
            _inventoryRepository = inventoryRepository;
            _inventoryBusinessRules = inventoryBusinessRules;
        }

        public async Task<GetByIdInventoryResponse> Handle(GetByIdInventoryQuery request, CancellationToken cancellationToken)
        {
            Inventory? inventory = await _inventoryRepository.GetAsync(predicate: i => i.Id == request.Id, cancellationToken: cancellationToken);
            await _inventoryBusinessRules.InventoryShouldExistWhenSelected(inventory);

            GetByIdInventoryResponse response = _mapper.Map<GetByIdInventoryResponse>(inventory);
            return response;
        }
    }
}