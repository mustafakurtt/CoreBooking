using Domain.Entities;
using NArchitecture.Core.Persistence.Repositories;

namespace Application.Services.Repositories;

public interface IGuestRepository : IAsyncRepository<Guest, Guid>, IRepository<Guest, Guid>
{
}