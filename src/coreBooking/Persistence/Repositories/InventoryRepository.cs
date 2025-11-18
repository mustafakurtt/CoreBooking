using Application.Services.Repositories;
using Domain.Entities;
using NArchitecture.Core.Persistence.Repositories;
using Persistence.Contexts;

namespace Persistence.Repositories;

public class InventoryRepository : EfRepositoryBase<Inventory, Guid, BaseDbContext>, IInventoryRepository
{
    public InventoryRepository(BaseDbContext context) : base(context)
    {
    }
}