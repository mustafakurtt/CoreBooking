using Application.Features.Bookings.Constants;
using Application.Services.Repositories;
using NArchitecture.Core.Application.Rules;
using NArchitecture.Core.CrossCuttingConcerns.Exception.Types;
using NArchitecture.Core.Localization.Abstraction;
using Domain.Entities;

namespace Application.Features.Bookings.Rules;

public class BookingBusinessRules : BaseBusinessRules
{
    private readonly IBookingRepository _bookingRepository;
    private readonly ILocalizationService _localizationService;

    public BookingBusinessRules(IBookingRepository bookingRepository, ILocalizationService localizationService)
    {
        _bookingRepository = bookingRepository;
        _localizationService = localizationService;
    }

    private async Task throwBusinessException(string messageKey)
    {
        string message = await _localizationService.GetLocalizedAsync(messageKey, BookingsBusinessMessages.SectionName);
        throw new BusinessException(message);
    }

    public async Task BookingShouldExistWhenSelected(Booking? booking)
    {
        if (booking == null)
            await throwBusinessException(BookingsBusinessMessages.BookingNotExists);
    }

    public async Task BookingIdShouldExistWhenSelected(Guid id, CancellationToken cancellationToken)
    {
        Booking? booking = await _bookingRepository.GetAsync(
            predicate: b => b.Id == id,
            enableTracking: false,
            cancellationToken: cancellationToken
        );
        await BookingShouldExistWhenSelected(booking);
    }
}