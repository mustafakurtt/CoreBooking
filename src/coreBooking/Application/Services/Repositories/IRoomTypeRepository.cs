using Domain.Entities;
using NArchitecture.Core.Persistence.Repositories;

namespace Application.Services.Repositories;

public interface IRoomTypeRepository : IAsyncRepository<RoomType, Guid>, IRepository<RoomType, Guid>
{
}