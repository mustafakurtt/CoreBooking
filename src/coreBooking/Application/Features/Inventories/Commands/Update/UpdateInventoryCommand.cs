using Application.Features.Inventories.Constants;
using Application.Features.Inventories.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using Domain.ValueObjects;
using MediatR;
using NArchitecture.Core.Application.Pipelines.Authorization;
using NArchitecture.Core.Application.Pipelines.Logging;
using NArchitecture.Core.Application.Pipelines.Transaction;
using Shared.Enums;
using static Application.Features.Inventories.Constants.InventoriesOperationClaims;

namespace Application.Features.Inventories.Commands.Update;

public class UpdateInventoryCommand : IRequest<UpdatedInventoryResponse>, ISecuredRequest, ILoggableRequest, ITransactionalRequest
{
    public Guid Id { get; set; }
    public required Guid RoomTypeId { get; set; }
    public required DateTime Date { get; set; }
    public required int Quantity { get; set; }

    // --- V2 GÜNCELLEMESÝ: Flat Input ---
    public required decimal PriceAmount { get; set; }
    public required Currency PriceCurrency { get; set; }

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
            // 1. Kaydý Getir
            Inventory? inventory = await _inventoryRepository.GetAsync(
                predicate: i => i.Id == request.Id,
                cancellationToken: cancellationToken
            );

            // 2. TEMEL KONTROL: Kayýt var mý?
            await _inventoryBusinessRules.InventoryShouldExistWhenSelected(inventory);

            // 3. KURAL: Geçmiþ tarihli stok güncellenemez (Veri tutarlýlýðý)
            // (Hem eski tarih hem yeni tarih kontrol edilebilir, þimdilik yeni tarihi kontrol edelim)
            await _inventoryBusinessRules.InventoryDateShouldNotBeInPast(request.Date);

            // 4. KURAL: Eðer Oda Tipi veya Tarih deðiþiyorsa, çakýþma var mý?
            if (inventory!.RoomTypeId != request.RoomTypeId || inventory.Date != request.Date)
            {
                await _inventoryBusinessRules.InventoryShouldNotExistsWhenUpdate(request.Id, request.RoomTypeId, request.Date, cancellationToken);
            }

            // 5. Mapping (Basit alanlar)
            _mapper.Map(request, inventory);

            // 6. DOMAIN LOGIC: Money Value Object Güncelleme
            inventory.Price = new Money(request.PriceAmount, request.PriceCurrency);

            // 7. Veritabaný Güncelleme
            await _inventoryRepository.UpdateAsync(inventory!);

            // 8. Response
            UpdatedInventoryResponse response = _mapper.Map<UpdatedInventoryResponse>(inventory);
            return response;
        }
    }
}