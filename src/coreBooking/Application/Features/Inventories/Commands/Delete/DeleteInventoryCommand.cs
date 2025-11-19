using Application.Features.Inventories.Constants;
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

namespace Application.Features.Inventories.Commands.Delete;

public class DeleteInventoryCommand : IRequest<DeletedInventoryResponse>, ISecuredRequest, ILoggableRequest, ITransactionalRequest
{
    public Guid Id { get; set; }

    public string[] Roles => [Admin, Write, InventoriesOperationClaims.Delete];

    public class DeleteInventoryCommandHandler : IRequestHandler<DeleteInventoryCommand, DeletedInventoryResponse>
    {
        private readonly IMapper _mapper;
        private readonly IInventoryRepository _inventoryRepository;
        private readonly InventoryBusinessRules _inventoryBusinessRules;

        public DeleteInventoryCommandHandler(IMapper mapper, IInventoryRepository inventoryRepository,
            InventoryBusinessRules inventoryBusinessRules)
        {
            _mapper = mapper;
            _inventoryRepository = inventoryRepository;
            _inventoryBusinessRules = inventoryBusinessRules;
        }

        public async Task<DeletedInventoryResponse> Handle(DeleteInventoryCommand request, CancellationToken cancellationToken)
        {
            // 1. Kaydý Getir
            Inventory? inventory = await _inventoryRepository.GetAsync(
                predicate: i => i.Id == request.Id,
                cancellationToken: cancellationToken
            );

            // 2. Varlýk Kontrolü
            await _inventoryBusinessRules.InventoryShouldExistWhenSelected(inventory);

            // 3. KURAL: Geçmiþ stok verisi silinemez (Rapor güvenliði)
            // Bu kuralý daha önce InventoryBusinessRules içine eklemiþtik.
            await _inventoryBusinessRules.InventoryDateShouldNotBeInPast(inventory!.Date);

            // 4. Sil (Soft Delete)
            await _inventoryRepository.DeleteAsync(inventory!);

            // 5. Response
            DeletedInventoryResponse response = _mapper.Map<DeletedInventoryResponse>(inventory);
            return response;
        }
    }
}