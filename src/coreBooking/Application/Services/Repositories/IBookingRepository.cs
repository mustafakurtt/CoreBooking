using Domain.Entities;
using NArchitecture.Core.Persistence.Repositories;

namespace Application.Services.Repositories;

public interface IBookingRepository : IAsyncRepository<Booking, Guid>, IRepository<Booking, Guid>
{
}