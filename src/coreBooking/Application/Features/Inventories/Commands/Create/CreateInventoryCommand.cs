using Application.Features.Inventories.Constants;
using Application.Features.Inventories.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities; // Namespace
using Domain.ValueObjects; // Money için
using Shared.Enums;
using MediatR;
using NArchitecture.Core.Application.Pipelines.Authorization;
using NArchitecture.Core.Application.Pipelines.Logging;
using NArchitecture.Core.Application.Pipelines.Transaction;
using static Application.Features.Inventories.Constants.InventoriesOperationClaims;

namespace Application.Features.Inventories.Commands.Create;

public class CreateInventoryCommand : IRequest<CreatedInventoryResponse>, ISecuredRequest, ILoggableRequest, ITransactionalRequest
{
    public required Guid RoomTypeId { get; set; }
    public required DateTime Date { get; set; }
    public required int Quantity { get; set; }

    // --- V2: Flat Input ---
    public required decimal PriceAmount { get; set; }
    public required Currency PriceCurrency { get; set; }

    public string[] Roles => [Admin, Write, InventoriesOperationClaims.Create];

    public class CreateInventoryCommandHandler : IRequestHandler<CreateInventoryCommand, CreatedInventoryResponse>
    {
        private readonly IMapper _mapper;
        private readonly IInventoryRepository _inventoryRepository;
        private readonly InventoryBusinessRules _inventoryBusinessRules;

        public CreateInventoryCommandHandler(IMapper mapper, IInventoryRepository inventoryRepository,
                                             InventoryBusinessRules inventoryBusinessRules)
        {
            _mapper = mapper;
            _inventoryRepository = inventoryRepository;
            _inventoryBusinessRules = inventoryBusinessRules;
        }

        public async Task<CreatedInventoryResponse> Handle(CreateInventoryCommand request, CancellationToken cancellationToken)
        {
            await _inventoryBusinessRules.RoomTypeIdShouldExist(request.RoomTypeId, cancellationToken);

            await _inventoryBusinessRules.InventoryDateShouldNotBeInPast(request.Date);

            await _inventoryBusinessRules.InventoryShouldNotExistsWhenInsert(request.RoomTypeId, request.Date, cancellationToken);

            Money price = new(request.PriceAmount, request.PriceCurrency);

            Inventory inventory = new()
            {
                Id = Guid.NewGuid(),
                RoomTypeId = request.RoomTypeId,
                Date = request.Date,
                Quantity = request.Quantity,
                Price = price
            };

            await _inventoryRepository.AddAsync(inventory);

            CreatedInventoryResponse response = _mapper.Map<CreatedInventoryResponse>(inventory);
            return response;
        }
    }
}