using Application.Services.Repositories;
using Domain.Entities;
using NArchitecture.Core.Persistence.Repositories;
using Persistence.Contexts;

namespace Persistence.Repositories;

public class GuestRepository : EfRepositoryBase<Guest, Guid, BaseDbContext>, IGuestRepository
{
    public GuestRepository(BaseDbContext context) : base(context)
    {
    }
}