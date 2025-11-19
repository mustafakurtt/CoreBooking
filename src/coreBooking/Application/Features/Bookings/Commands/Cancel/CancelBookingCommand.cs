using Application.Features.Bookings.Constants;
using Application.Features.Bookings.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NArchitecture.Core.Application.Pipelines.Authorization;
using NArchitecture.Core.Application.Pipelines.Caching;
using NArchitecture.Core.Application.Pipelines.Logging;
using NArchitecture.Core.Application.Pipelines.Transaction;
using NArchitecture.Core.Security.Constants;
using Shared.Enums;
using System.ComponentModel;

namespace Application.Features.Bookings.Commands.Cancel;

public class CancelBookingCommand : IRequest<CanceledBookingResponse>,
    ISecuredRequest,       // 1. Sadece yetkili kişiler (Admin)
    ILoggableRequest,      // 2. "Kim iptal etti?" logunu tut
    ITransactionalRequest, // 3. Booking iptal olurken Inventory artmazsa işlemi geri al (Rollback)
    ICacheRemoverRequest   // 4. Stok değişti, arama sonuçlarının önbelleğini temizle
{
    public Guid Id { get; set; }

    public string[] Roles => [GeneralOperationClaims.Admin, BookingsOperationClaims.Write, BookingsOperationClaims.Delete];

    public bool BypassCache { get; }
    public string? CacheKey => null;
    public string[]? CacheGroupKey => ["GetAvailableRooms"];

    public class CancelBookingCommandHandler : IRequestHandler<CancelBookingCommand, CanceledBookingResponse>
    {
        private readonly IMapper _mapper;
        private readonly IBookingRepository _bookingRepository;
        private readonly IInventoryRepository _inventoryRepository;
        private readonly BookingBusinessRules _bookingBusinessRules;

        public CancelBookingCommandHandler(
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

        public async Task<CanceledBookingResponse> Handle(CancelBookingCommand request, CancellationToken cancellationToken)
        {
            Booking? booking = await _bookingRepository.GetAsync(
                predicate: b => b.Id == request.Id,
                include: b => b.Include(x => x.Payments),
                cancellationToken: cancellationToken
            );

            await _bookingBusinessRules.BookingShouldExistWhenSelected(booking);

            booking!.Cancel();

            var inventories = await _inventoryRepository.GetListAsync(
                predicate: i => i.RoomTypeId == booking.RoomTypeId &&
                                i.Date >= booking.Period.Start &&
                                i.Date < booking.Period.End,
                cancellationToken: cancellationToken
            );

            foreach (var inventory in inventories.Items)
            {
                inventory.Quantity += 1;
            }

            foreach (var payment in booking.Payments)
            {
                if (payment.Status == PaymentStatus.Completed)
                {
                    payment.Status = PaymentStatus.Refunded;
                }
            }
            await _bookingRepository.UpdateAsync(booking);
            await _inventoryRepository.UpdateRangeAsync(inventories.Items);

            CanceledBookingResponse response = _mapper.Map<CanceledBookingResponse>(booking);
            return response;
        }
    }
}