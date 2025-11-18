using Application.Services.Repositories;
using Domain.Entities;
using NArchitecture.Core.Persistence.Repositories;
using Persistence.Contexts;

namespace Persistence.Repositories;

public class RoomTypeRepository : EfRepositoryBase<RoomType, Guid, BaseDbContext>, IRoomTypeRepository
{
    public RoomTypeRepository(BaseDbContext context) : base(context)
    {
    }
}