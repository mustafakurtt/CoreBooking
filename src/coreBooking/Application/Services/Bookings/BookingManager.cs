using Application.Features.Bookings.Rules;
using Application.Services.Repositories;
using NArchitecture.Core.Persistence.Paging;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Application.Services.Bookings;

public class BookingManager : IBookingService
{
    private readonly IBookingRepository _bookingRepository;
    private readonly BookingBusinessRules _bookingBusinessRules;

    public BookingManager(IBookingRepository bookingRepository, BookingBusinessRules bookingBusinessRules)
    {
        _bookingRepository = bookingRepository;
        _bookingBusinessRules = bookingBusinessRules;
    }

    public async Task<Booking?> GetAsync(
        Expression<Func<Booking, bool>> predicate,
        Func<IQueryable<Booking>, IIncludableQueryable<Booking, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    )
    {
        Booking? booking = await _bookingRepository.GetAsync(predicate, include, withDeleted, enableTracking, cancellationToken);
        return booking;
    }

    public async Task<IPaginate<Booking>?> GetListAsync(
        Expression<Func<Booking, bool>>? predicate = null,
        Func<IQueryable<Booking>, IOrderedQueryable<Booking>>? orderBy = null,
        Func<IQueryable<Booking>, IIncludableQueryable<Booking, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    )
    {
        IPaginate<Booking> bookingList = await _bookingRepository.GetListAsync(
            predicate,
            orderBy,
            include,
            index,
            size,
            withDeleted,
            enableTracking,
            cancellationToken
        );
        return bookingList;
    }

    public async Task<Booking> AddAsync(Booking booking)
    {
        Booking addedBooking = await _bookingRepository.AddAsync(booking);

        return addedBooking;
    }

    public async Task<Booking> UpdateAsync(Booking booking)
    {
        Booking updatedBooking = await _bookingRepository.UpdateAsync(booking);

        return updatedBooking;
    }

    public async Task<Booking> DeleteAsync(Booking booking, bool permanent = false)
    {
        Booking deletedBooking = await _bookingRepository.DeleteAsync(booking);

        return deletedBooking;
    }
}
