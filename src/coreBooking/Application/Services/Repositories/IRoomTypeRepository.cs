using Application.Features.RoomTypes.Queries.GetAvailableRooms;
using Domain.Entities;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Persistence.Dynamic;
using NArchitecture.Core.Persistence.Paging;
using NArchitecture.Core.Persistence.Repositories;

namespace Application.Services.Repositories;

public interface IRoomTypeRepository : IAsyncRepository<RoomType, Guid>, IRepository<RoomType, Guid>
{
    Task<IPaginate<GetAvailableRoomsResponse>> GetAvailableRoomsAsync(
        DateTime checkInDate,
        DateTime checkOutDate,
        int numberOfGuests,
        PageRequest pageRequest,
        DynamicQuery? dynamicQuery = null,
        CancellationToken cancellationToken = default
    );
}