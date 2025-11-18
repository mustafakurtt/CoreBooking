using Application.Features.Bookings.Constants;
using Application.Features.Bookings.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using NArchitecture.Core.Application.Pipelines.Authorization;
using NArchitecture.Core.Application.Pipelines.Logging;
using NArchitecture.Core.Application.Pipelines.Transaction;
using MediatR;
using Domain.Enums;
using static Application.Features.Bookings.Constants.BookingsOperationClaims;
using NArchitecture.Core.CrossCuttingConcerns.Exception.Types;

namespace Application.Features.Bookings.Commands.Create;

public class CreateBookingCommand : IRequest<CreatedBookingResponse>, ISecuredRequest, ILoggableRequest, ITransactionalRequest
{
    // Sadece kullanýcýnýn girmesi gerekenler kalmalý:
    public required Guid CustomerId { get; set; }
    public required Guid RoomTypeId { get; set; }
    public required DateTime CheckInDate { get; set; }
    public required DateTime CheckOutDate { get; set; }
    public required int NumberOfGuests { get; set; }

    // SÝLÝNENLER: TotalPrice ve Status artýk yok.

    public string[] Roles => [Admin, Write, BookingsOperationClaims.Create];

    public class CreateBookingCommandHandler : IRequestHandler<CreateBookingCommand, CreatedBookingResponse>
    {
        private readonly IMapper _mapper;
        private readonly IBookingRepository _bookingRepository;
        private readonly IInventoryRepository _inventoryRepository; // EKLENDÝ
        private readonly BookingBusinessRules _bookingBusinessRules;

        public CreateBookingCommandHandler(
            IMapper mapper,
            IBookingRepository bookingRepository,
            IInventoryRepository inventoryRepository, // EKLENDÝ
            BookingBusinessRules bookingBusinessRules)
        {
            _mapper = mapper;
            _bookingRepository = bookingRepository;
            _inventoryRepository = inventoryRepository;
            _bookingBusinessRules = bookingBusinessRules;
        }

        public async Task<CreatedBookingResponse> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
        {
            // 1. Envanterleri Çek
            var inventories = await _inventoryRepository.GetListAsync(
                predicate: i => i.RoomTypeId == request.RoomTypeId &&
                                i.Date >= request.CheckInDate.Date &&
                                i.Date < request.CheckOutDate.Date,
                cancellationToken: cancellationToken
            );

            // 2. Gün Sayýsý Kontrolü
            var totalDays = (request.CheckOutDate.Date - request.CheckInDate.Date).Days;
            if (inventories.Count != totalDays)
                throw new BusinessException("Seçilen tarih aralýðý için uygun fiyat/stok bilgisi bulunamadý.");

            // 3. Stok ve Fiyat Hesaplama
            decimal calculatedTotalPrice = 0;
            foreach (var dayInventory in inventories.Items)
            {
                if (dayInventory.Quantity <= 0)
                    throw new BusinessException($"{dayInventory.Date.ToShortDateString()} tarihinde odalar tükendi.");

                dayInventory.Quantity -= 1; // Stoktan düþ
                calculatedTotalPrice += dayInventory.Price; // Fiyatý topla
            }

            // 4. Manuel Booking Oluþturma (Mapper kullanmýyoruz çünkü custom logic var)
            Booking booking = new()
            {
                Id = Guid.NewGuid(),
                CustomerId = request.CustomerId,
                RoomTypeId = request.RoomTypeId,
                CheckInDate = request.CheckInDate,
                CheckOutDate = request.CheckOutDate,
                NumberOfGuests = request.NumberOfGuests,
                TotalPrice = calculatedTotalPrice, // Hesaplanan fiyatý buraya basýyoruz
                Status = BookingStatus.Confirmed
            };

            // 5. Kaydet (Inventory update'i de transaction içinde otomatik yapýlacak)
            await _bookingRepository.AddAsync(booking);
            await _inventoryRepository.UpdateRangeAsync(inventories.Items);

            // 6. Response Dön
            CreatedBookingResponse response = _mapper.Map<CreatedBookingResponse>(booking);
            return response;
        }
    }
}