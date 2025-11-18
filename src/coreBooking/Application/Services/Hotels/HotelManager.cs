using Application.Features.Hotels.Rules;
using Application.Services.Repositories;
using NArchitecture.Core.Persistence.Paging;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Application.Services.Hotels;

public class HotelManager : IHotelService
{
    private readonly IHotelRepository _hotelRepository;
    private readonly HotelBusinessRules _hotelBusinessRules;

    public HotelManager(IHotelRepository hotelRepository, HotelBusinessRules hotelBusinessRules)
    {
        _hotelRepository = hotelRepository;
        _hotelBusinessRules = hotelBusinessRules;
    }

    public async Task<Hotel?> GetAsync(
        Expression<Func<Hotel, bool>> predicate,
        Func<IQueryable<Hotel>, IIncludableQueryable<Hotel, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    )
    {
        Hotel? hotel = await _hotelRepository.GetAsync(predicate, include, withDeleted, enableTracking, cancellationToken);
        return hotel;
    }

    public async Task<IPaginate<Hotel>?> GetListAsync(
        Expression<Func<Hotel, bool>>? predicate = null,
        Func<IQueryable<Hotel>, IOrderedQueryable<Hotel>>? orderBy = null,
        Func<IQueryable<Hotel>, IIncludableQueryable<Hotel, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    )
    {
        IPaginate<Hotel> hotelList = await _hotelRepository.GetListAsync(
            predicate,
            orderBy,
            include,
            index,
            size,
            withDeleted,
            enableTracking,
            cancellationToken
        );
        return hotelList;
    }

    public async Task<Hotel> AddAsync(Hotel hotel)
    {
        Hotel addedHotel = await _hotelRepository.AddAsync(hotel);

        return addedHotel;
    }

    public async Task<Hotel> UpdateAsync(Hotel hotel)
    {
        Hotel updatedHotel = await _hotelRepository.UpdateAsync(hotel);

        return updatedHotel;
    }

    public async Task<Hotel> DeleteAsync(Hotel hotel, bool permanent = false)
    {
        Hotel deletedHotel = await _hotelRepository.DeleteAsync(hotel);

        return deletedHotel;
    }
}
