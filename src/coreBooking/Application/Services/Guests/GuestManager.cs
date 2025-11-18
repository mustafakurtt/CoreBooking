using Application.Features.Guests.Rules;
using Application.Services.Repositories;
using NArchitecture.Core.Persistence.Paging;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Application.Services.Guests;

public class GuestManager : IGuestService
{
    private readonly IGuestRepository _guestRepository;
    private readonly GuestBusinessRules _guestBusinessRules;

    public GuestManager(IGuestRepository guestRepository, GuestBusinessRules guestBusinessRules)
    {
        _guestRepository = guestRepository;
        _guestBusinessRules = guestBusinessRules;
    }

    public async Task<Guest?> GetAsync(
        Expression<Func<Guest, bool>> predicate,
        Func<IQueryable<Guest>, IIncludableQueryable<Guest, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    )
    {
        Guest? guest = await _guestRepository.GetAsync(predicate, include, withDeleted, enableTracking, cancellationToken);
        return guest;
    }

    public async Task<IPaginate<Guest>?> GetListAsync(
        Expression<Func<Guest, bool>>? predicate = null,
        Func<IQueryable<Guest>, IOrderedQueryable<Guest>>? orderBy = null,
        Func<IQueryable<Guest>, IIncludableQueryable<Guest, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    )
    {
        IPaginate<Guest> guestList = await _guestRepository.GetListAsync(
            predicate,
            orderBy,
            include,
            index,
            size,
            withDeleted,
            enableTracking,
            cancellationToken
        );
        return guestList;
    }

    public async Task<Guest> AddAsync(Guest guest)
    {
        Guest addedGuest = await _guestRepository.AddAsync(guest);

        return addedGuest;
    }

    public async Task<Guest> UpdateAsync(Guest guest)
    {
        Guest updatedGuest = await _guestRepository.UpdateAsync(guest);

        return updatedGuest;
    }

    public async Task<Guest> DeleteAsync(Guest guest, bool permanent = false)
    {
        Guest deletedGuest = await _guestRepository.DeleteAsync(guest);

        return deletedGuest;
    }
}
