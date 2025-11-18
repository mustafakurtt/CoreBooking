using Application.Features.RoomTypes.Rules;
using Application.Services.Repositories;
using NArchitecture.Core.Persistence.Paging;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Application.Services.RoomTypes;

public class RoomTypeManager : IRoomTypeService
{
    private readonly IRoomTypeRepository _roomTypeRepository;
    private readonly RoomTypeBusinessRules _roomTypeBusinessRules;

    public RoomTypeManager(IRoomTypeRepository roomTypeRepository, RoomTypeBusinessRules roomTypeBusinessRules)
    {
        _roomTypeRepository = roomTypeRepository;
        _roomTypeBusinessRules = roomTypeBusinessRules;
    }

    public async Task<RoomType?> GetAsync(
        Expression<Func<RoomType, bool>> predicate,
        Func<IQueryable<RoomType>, IIncludableQueryable<RoomType, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    )
    {
        RoomType? roomType = await _roomTypeRepository.GetAsync(predicate, include, withDeleted, enableTracking, cancellationToken);
        return roomType;
    }

    public async Task<IPaginate<RoomType>?> GetListAsync(
        Expression<Func<RoomType, bool>>? predicate = null,
        Func<IQueryable<RoomType>, IOrderedQueryable<RoomType>>? orderBy = null,
        Func<IQueryable<RoomType>, IIncludableQueryable<RoomType, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    )
    {
        IPaginate<RoomType> roomTypeList = await _roomTypeRepository.GetListAsync(
            predicate,
            orderBy,
            include,
            index,
            size,
            withDeleted,
            enableTracking,
            cancellationToken
        );
        return roomTypeList;
    }

    public async Task<RoomType> AddAsync(RoomType roomType)
    {
        RoomType addedRoomType = await _roomTypeRepository.AddAsync(roomType);

        return addedRoomType;
    }

    public async Task<RoomType> UpdateAsync(RoomType roomType)
    {
        RoomType updatedRoomType = await _roomTypeRepository.UpdateAsync(roomType);

        return updatedRoomType;
    }

    public async Task<RoomType> DeleteAsync(RoomType roomType, bool permanent = false)
    {
        RoomType deletedRoomType = await _roomTypeRepository.DeleteAsync(roomType);

        return deletedRoomType;
    }
}
