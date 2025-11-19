using Application.Features.RoomTypes.Commands.Create;
using Application.Features.RoomTypes.Commands.Delete;
using Application.Features.RoomTypes.Commands.Update;
using Application.Features.RoomTypes.Queries.GetById;
using Application.Features.RoomTypes.Queries.GetList;
using Application.Features.RoomTypes.Queries.GetListByDynamic;
using AutoMapper;
using NArchitecture.Core.Application.Responses;
using Domain.Entities;
using NArchitecture.Core.Persistence.Paging;

namespace Application.Features.RoomTypes.Profiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<CreateRoomTypeCommand, RoomType>();
        CreateMap<RoomType, CreatedRoomTypeResponse>();

        // Update
        CreateMap<UpdateRoomTypeCommand, RoomType>();
        CreateMap<RoomType, UpdatedRoomTypeResponse>();

        // Delete
        CreateMap<DeleteRoomTypeCommand, RoomType>();
        CreateMap<RoomType, DeletedRoomTypeResponse>();


        // ---------------- QUERIES (Okuma Ýþlemleri) ----------------

        // GetById Mapping (Zenginleþtirilmiþ: Hotel Adý ile)
        CreateMap<RoomType, GetByIdRoomTypeResponse>()
            .ForMember(dest => dest.HotelName, opt => opt.MapFrom(src => src.Hotel.Name));

        // GetList Mapping (Zenginleþtirilmiþ: Hotel Adý ile)
        CreateMap<RoomType, GetListRoomTypeListItemDto>()
            .ForMember(dest => dest.HotelName, opt => opt.MapFrom(src => src.Hotel.Name));

        CreateMap<IPaginate<RoomType>, GetListResponse<GetListRoomTypeListItemDto>>();

        // GetListByDynamic Mapping (Zenginleþtirilmiþ: Hotel Adý ile)
        CreateMap<RoomType, GetListByDynamicRoomTypeListItemDto>()
            .ForMember(dest => dest.HotelName, opt => opt.MapFrom(src => src.Hotel.Name));

        CreateMap<IPaginate<RoomType>, GetListResponse<GetListByDynamicRoomTypeListItemDto>>();
    }
}