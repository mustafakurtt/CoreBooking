using Domain.Entities;
using NArchitecture.Core.Persistence.Repositories;

namespace Application.Services.Repositories;

public interface IInventoryRepository : IAsyncRepository<Inventory, Guid>, IRepository<Inventory, Guid>
{
}