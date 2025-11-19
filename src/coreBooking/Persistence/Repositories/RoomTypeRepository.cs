using Application.Features.RoomTypes.Queries.GetAvailableRooms;
using Application.Services.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Persistence.Dynamic;
using NArchitecture.Core.Persistence.Paging;
using NArchitecture.Core.Persistence.Repositories;
using Persistence.Contexts;

namespace Persistence.Repositories;

public class RoomTypeRepository : EfRepositoryBase<RoomType, Guid, BaseDbContext>, IRoomTypeRepository
{
    public RoomTypeRepository(BaseDbContext context) : base(context)
    {
    }

    public async Task<IPaginate<GetAvailableRoomsResponse>> GetAvailableRoomsAsync(
        DateTime checkInDate,
        DateTime checkOutDate,
        int numberOfGuests,
        PageRequest pageRequest,
        DynamicQuery? dynamicQuery = null,
        CancellationToken cancellationToken = default)
    {
        int totalDays = (checkOutDate.Date - checkInDate.Date).Days;
        if (totalDays <= 0) return new Paginate<GetAvailableRoomsResponse>(); // Boþ dön

        var query =
            from rt in Context.RoomTypes
            join h in Context.Hotels on rt.HotelId equals h.Id
            join i in Context.Inventories on rt.Id equals i.RoomTypeId

            where i.Date >= checkInDate.Date && i.Date < checkOutDate.Date
            where i.Quantity > 0
            where rt.Capacity >= numberOfGuests

            group i by new { rt.Id, RoomTypeName = rt.Name, rt.HotelId, HotelName = h.Name, h.City, h.Address, rt.Capacity } into g

            where g.Count() == totalDays

            select new GetAvailableRoomsResponse
            {
                RoomTypeId = g.Key.Id,
                RoomTypeName = g.Key.RoomTypeName,
                HotelId = g.Key.HotelId,
                HotelName = g.Key.HotelName,
                HotelCity = g.Key.City,

                TotalPriceAmount = g.Sum(x => x.Price.Amount),

                Currency = g.Select(x => x.Price.Currency).FirstOrDefault()
            };

        if (dynamicQuery?.Sort != null || dynamicQuery?.Filter != null)
        {
            query = query.ToDynamic(dynamicQuery);
        }
        IPaginate<GetAvailableRoomsResponse> result = await query.ToPaginateAsync(
            index: pageRequest.PageIndex,
            size: pageRequest.PageSize,
            from: 0,
            cancellationToken: cancellationToken
        );

        return result;
    }
}