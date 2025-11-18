using Application.Services.Repositories;
using Domain.Entities;
using NArchitecture.Core.Persistence.Repositories;
using Persistence.Contexts;

namespace Persistence.Repositories;

public class HotelRepository : EfRepositoryBase<Hotel, Guid, BaseDbContext>, IHotelRepository
{
    public HotelRepository(BaseDbContext context) : base(context)
    {
    }
}