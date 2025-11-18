using Application.Features.Inventories.Constants;
using Application.Features.Inventories.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using NArchitecture.Core.Application.Pipelines.Authorization;
using NArchitecture.Core.Application.Pipelines.Logging;
using NArchitecture.Core.Application.Pipelines.Transaction;
using MediatR;
using static Application.Features.Inventories.Constants.InventoriesOperationClaims;

namespace Application.Features.Inventories.Commands.Update;

public class UpdateInventoryCommand : IRequest<UpdatedInventoryResponse>, ISecuredRequest, ILoggableRequest, ITransactionalRequest
{
    public Guid Id { get; set; }
    public required Guid RoomTypeId { get; set; }
    public required DateTime Date { get; set; }
    public required int Quantity { get; set; }
    public required decimal Price { get; set; }

    public string[] Roles => [Admin, Write, InventoriesOperationClaims.Update];

    public class UpdateInventoryCommandHandler : IRequestHandler<UpdateInventoryCommand, UpdatedInventoryResponse>
    {
        private readonly IMapper _mapper;
        private readonly IInventoryRepository _inventoryRepository;
        private readonly InventoryBusinessRules _inventoryBusinessRules;

        public UpdateInventoryCommandHandler(IMapper mapper, IInventoryRepository inventoryRepository,
                                         InventoryBusinessRules inventoryBusinessRules)
        {
            _mapper = mapper;
            _inventoryRepository = inventoryRepository;
            _inventoryBusinessRules = inventoryBusinessRules;
        }

        public async Task<UpdatedInventoryResponse> Handle(UpdateInventoryCommand request, CancellationToken cancellationToken)
        {
            Inventory? inventory = await _inventoryRepository.GetAsync(predicate: i => i.Id == request.Id, cancellationToken: cancellationToken);
            await _inventoryBusinessRules.InventoryShouldExistWhenSelected(inventory);
            inventory = _mapper.Map(request, inventory);

            await _inventoryRepository.UpdateAsync(inventory!);

            UpdatedInventoryResponse response = _mapper.Map<UpdatedInventoryResponse>(inventory);
            return response;
        }
    }
}