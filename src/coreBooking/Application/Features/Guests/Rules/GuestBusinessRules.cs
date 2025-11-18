using Application.Features.Guests.Constants;
using Application.Services.Repositories;
using Domain.Entities;
using NArchitecture.Core.Application.Rules;
using NArchitecture.Core.CrossCuttingConcerns.Exception.Types;
using NArchitecture.Core.Localization.Abstraction;
using Shared.Constants;
using Shared.Enums;

namespace Application.Features.Guests.Rules;

public class GuestBusinessRules : BaseBusinessRules
{
    private readonly IGuestRepository _guestRepository;
    private readonly IBookingRepository _bookingRepository;
    private readonly ILocalizationService _localizationService;

    public GuestBusinessRules(
        IGuestRepository guestRepository,
        IBookingRepository bookingRepository,
        ILocalizationService localizationService)
    {
        _guestRepository = guestRepository;
        _bookingRepository = bookingRepository;
        _localizationService = localizationService;
    }

    private async Task throwBusinessException(string messageKey)
    {
        string message = await _localizationService.GetLocalizedAsync(messageKey, GuestsBusinessMessages.SectionName);
        throw new BusinessException(message);
    }
    public async Task GuestShouldExistWhenSelected(Guest? guest)
    {
        if (guest == null)
            await throwBusinessException(GuestsBusinessMessages.GuestNotExists);
    }

    public async Task GuestIdShouldExistWhenSelected(Guid id, CancellationToken cancellationToken)
    {
        Guest? guest = await _guestRepository.GetAsync(
            predicate: g => g.Id == id,
            enableTracking: false,
            cancellationToken: cancellationToken
        );
        await GuestShouldExistWhenSelected(guest);
    }

    public async Task BookingIdShouldExist(Guid bookingId, CancellationToken cancellationToken)
    {
        bool doesExist = await _bookingRepository.AnyAsync(
            predicate: b => b.Id == bookingId,
            cancellationToken: cancellationToken
        );

        if (!doesExist)
            await throwBusinessException(GuestsBusinessMessages.BookingNotExists);
    }

    public async Task BookingShouldBeActive(Guid bookingId, CancellationToken cancellationToken)
    {
        Booking? booking = await _bookingRepository.GetAsync(
            predicate: b => b.Id == bookingId,
            enableTracking: false,
            cancellationToken: cancellationToken
        );

        if (booking == null) return;

        if (booking.Status == BookingStatus.Cancelled || booking.Period.End < DateTime.UtcNow)
        {
            await throwBusinessException(GuestsBusinessMessages.BookingNotActive);
        }
    }

    public async Task BookingCapacityShouldNotBeExceeded(Guid bookingId, CancellationToken cancellationToken)
    {
        // 1. Rezervasyonu çek
        Booking? booking = await _bookingRepository.GetAsync(
            predicate: b => b.Id == bookingId,
            enableTracking: false,
            cancellationToken: cancellationToken
        );

        if (booking == null) return;

        // 2. PERFORMANSLI YÖNTEM: Özel repository metodunu çaðýr
        int currentGuestCount = await _guestRepository.GetGuestCountByBookingIdAsync(bookingId, cancellationToken);

        // 3. Kontrol
        if (currentGuestCount >= booking.NumberOfGuests)
        {
            await throwBusinessException(GuestsBusinessMessages.BookingCapacityExceeded);
        }
    }
}