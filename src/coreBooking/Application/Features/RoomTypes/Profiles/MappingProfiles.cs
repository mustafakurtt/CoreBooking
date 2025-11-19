using Application.Features.RoomTypes.Commands.Create;
using Application.Features.RoomTypes.Commands.Delete;
using Application.Features.RoomTypes.Commands.Update;
using Application.Features.RoomTypes.Queries.GetAvailableRooms;
using Application.Features.RoomTypes.Queries.GetById;
using Application.Features.RoomTypes.Queries.GetList;
using Application.Features.RoomTypes.Queries.GetListByDynamic;
using AutoMapper;
using Domain.Entities;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Paging;

namespace Application.Features.RoomTypes.Profiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<CreateRoomTypeCommand, RoomType>();
        CreateMap<RoomType, CreatedRoomTypeResponse>();

        CreateMap<UpdateRoomTypeCommand, RoomType>();
        CreateMap<RoomType, UpdatedRoomTypeResponse>();

        CreateMap<DeleteRoomTypeCommand, RoomType>();
        CreateMap<RoomType, DeletedRoomTypeResponse>();

        CreateMap<RoomType, GetByIdRoomTypeResponse>()
            .ForMember(dest => dest.HotelName, opt => opt.MapFrom(src => src.Hotel.Name));

        CreateMap<RoomType, GetListRoomTypeListItemDto>()
            .ForMember(dest => dest.HotelName, opt => opt.MapFrom(src => src.Hotel.Name));

        CreateMap<IPaginate<RoomType>, GetListResponse<GetListRoomTypeListItemDto>>();

        CreateMap<RoomType, GetListByDynamicRoomTypeListItemDto>()
            .ForMember(dest => dest.HotelName, opt => opt.MapFrom(src => src.Hotel.Name));

        CreateMap<IPaginate<RoomType>, GetListResponse<GetListByDynamicRoomTypeListItemDto>>();

        CreateMap<IPaginate<GetAvailableRoomsResponse>, GetListResponse<GetAvailableRoomsResponse>>();
    }
}