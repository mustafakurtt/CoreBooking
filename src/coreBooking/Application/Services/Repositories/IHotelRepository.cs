using Domain.Entities;
using NArchitecture.Core.Persistence.Repositories;

namespace Application.Services.Repositories;

public interface IHotelRepository : IAsyncRepository<Hotel, Guid>, IRepository<Hotel, Guid>
{
}