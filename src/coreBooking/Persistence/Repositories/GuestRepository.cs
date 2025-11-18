using Application.Services.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using NArchitecture.Core.Persistence.Repositories;
using Persistence.Contexts;

namespace Persistence.Repositories;

public class GuestRepository : EfRepositoryBase<Guest, Guid, BaseDbContext>, IGuestRepository
{
    public GuestRepository(BaseDbContext context) : base(context)
    {
    }

    public async Task<int> GetGuestCountByBookingIdAsync(Guid bookingId, CancellationToken cancellationToken)
    {

        return await Query()
            .Where(g => g.BookingId == bookingId)
            .CountAsync(cancellationToken);
    }
}