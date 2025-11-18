using Application.Services.Repositories;
using Domain.Entities;
using NArchitecture.Core.Persistence.Repositories;
using Persistence.Contexts;

namespace Persistence.Repositories;

public class BookingRepository : EfRepositoryBase<Booking, Guid, BaseDbContext>, IBookingRepository
{
    public BookingRepository(BaseDbContext context) : base(context)
    {
    }
}