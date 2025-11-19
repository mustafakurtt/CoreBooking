using Application.Features.Bookings.Constants;
using Application.Features.Bookings.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using Domain.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NArchitecture.Core.Application.Pipelines.Authorization;
using NArchitecture.Core.Application.Pipelines.Caching;
using NArchitecture.Core.Application.Pipelines.Logging;
using NArchitecture.Core.Application.Pipelines.Transaction;
using NArchitecture.Core.CrossCuttingConcerns.Exception.Types;
using Shared.Constants;
using Shared.Enums;
using static Application.Features.Bookings.Constants.BookingsOperationClaims;

namespace Application.Features.Bookings.Commands.Create;

public class CreateBookingCommand : IRequest<CreatedBookingResponse>, ISecuredRequest, ILoggableRequest, ITransactionalRequest, ICacheRemoverRequest
{
    public required Guid CustomerId { get; set; }
    public required Guid RoomTypeId { get; set; }
    public required DateTime CheckInDate { get; set; }
    public required DateTime CheckOutDate { get; set; }
    public required int NumberOfGuests { get; set; }

    public string[] Roles => [Admin, Write, BookingsOperationClaims.Create];

    public bool BypassCache { get; }
    public string? CacheKey => null;
    public string[]? CacheGroupKey => ["GetAvailableRooms"];

    public class CreateBookingCommandHandler : IRequestHandler<CreateBookingCommand, CreatedBookingResponse>
    {
        private readonly IMapper _mapper;
        private readonly IBookingRepository _bookingRepository;
        private readonly IInventoryRepository _inventoryRepository;
        private readonly BookingBusinessRules _bookingBusinessRules;

        public CreateBookingCommandHandler(
            IMapper mapper,
            IBookingRepository bookingRepository,
            IInventoryRepository inventoryRepository,
            BookingBusinessRules bookingBusinessRules)
        {
            _mapper = mapper;
            _bookingRepository = bookingRepository;
            _inventoryRepository = inventoryRepository;
            _bookingBusinessRules = bookingBusinessRules;
        }

        public async Task<CreatedBookingResponse> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
        {
            // 1. Value Object Oluþturma (Start < End validasyonu DateRange içinde yapýlýr)
            DateRange bookingPeriod = new(request.CheckInDate, request.CheckOutDate);

            // 2. Envanterleri Çek
            var inventories = await _inventoryRepository.GetListAsync(
                predicate: i => i.RoomTypeId == request.RoomTypeId &&
                                i.Date >= bookingPeriod.Start &&
                                i.Date < bookingPeriod.End,
                cancellationToken: cancellationToken
            );

            // 3. Gün Sayýsý Kontrolü
            if (inventories.Count != bookingPeriod.Days)
                throw new BusinessException("Stok/Fiyat bilgisi eksik olduðu için rezervasyon yapýlamýyor.");

            // 4. Stok ve Fiyat Hesaplama
            // Ýlk günün para birimini baz alýyoruz
            Currency baseCurrency = inventories.Items.First().Price.Currency;
            Money calculatedTotalPrice = new(0, baseCurrency);

            foreach (var dayInventory in inventories.Items)
            {
                // Stok kontrolü
                if (dayInventory.Quantity <= 0)
                    throw new BusinessException($"{dayInventory.Date.ToShortDateString()} tarihinde odalar tükendi.");

                // Para birimi kontrolü (Farklý para birimleri toplanamaz)
                if (dayInventory.Price.Currency != baseCurrency)
                    throw new BusinessException(ExceptionMessages.CurrenciesMustMatchForOperation);

                // Stoktan düþ (Memory update)
                dayInventory.Quantity -= 1;

                // Fiyatý topla (Money nesnesinin '+' operatörü çalýþýr)
                calculatedTotalPrice += dayInventory.Price;
            }

            // 5. Manuel Booking Oluþturma (V2)
            Booking booking = new()
            {
                Id = Guid.NewGuid(),
                CustomerId = request.CustomerId,
                RoomTypeId = request.RoomTypeId,
                Period = bookingPeriod, // Value Object
                NumberOfGuests = request.NumberOfGuests,
                TotalPrice = calculatedTotalPrice, // Value Object
                Status = BookingStatus.Confirmed
            };

            // 6. Kaydet (Optimistic Concurrency Korumasý ile)
            try
            {
                await _bookingRepository.AddAsync(booking);

                // Inventory deðiþikliklerini (stok düþümünü) veritabanýna yansýt
                await _inventoryRepository.UpdateRangeAsync(inventories.Items);
            }
            catch (DbUpdateConcurrencyException)
            {
                // "Son Oda Problemi" çakýþmasý
                throw new BusinessException("Ýþlem sýrasýnda seçtiðiniz odalardan biri baþkasý tarafýndan alýndý. Lütfen tekrar deneyin.");
            }

            // 7. Response Dön
            // NOT: MappingProfiles içinde Booking -> CreatedBookingResponse ayarýnýn yapýlmýþ olmasý gerekir.
            CreatedBookingResponse response = _mapper.Map<CreatedBookingResponse>(booking);
            return response;
        }
    }
}