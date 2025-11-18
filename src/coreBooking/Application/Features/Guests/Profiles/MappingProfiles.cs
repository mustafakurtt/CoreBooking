using Application.Features.Guests.Commands.Create;
using Application.Features.Guests.Commands.Delete;
using Application.Features.Guests.Commands.Update;
using Application.Features.Guests.Queries.GetById;
using Application.Features.Guests.Queries.GetList;
using Application.Features.Guests.Queries.GetListByDynamic;
using AutoMapper;
using NArchitecture.Core.Application.Responses;
using Domain.Entities;
using NArchitecture.Core.Persistence.Paging;

namespace Application.Features.Guests.Profiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<CreateGuestCommand, Guest>();
        CreateMap<Guest, CreatedGuestResponse>();

        CreateMap<UpdateGuestCommand, Guest>();
        CreateMap<Guest, UpdatedGuestResponse>();

        CreateMap<DeleteGuestCommand, Guest>();
        CreateMap<Guest, DeletedGuestResponse>();

        CreateMap<Guest, GetByIdGuestResponse>()
            .ForMember(dest => dest.CheckInDate, opt => opt.MapFrom(src => src.Booking.Period.Start))
            .ForMember(dest => dest.CheckOutDate, opt => opt.MapFrom(src => src.Booking.Period.End))
            .ForMember(dest => dest.RoomTypeName, opt => opt.MapFrom(src => src.Booking.RoomType.Name))
            .ForMember(dest => dest.HotelName, opt => opt.MapFrom(src => src.Booking.RoomType.Hotel.Name))
            .ForMember(dest => dest.HotelCity, opt => opt.MapFrom(src => src.Booking.RoomType.Hotel.City));


        CreateMap<Guest, GetListByDynamicGuestListItemDto>();
        CreateMap<IPaginate<Guest>, GetListResponse<GetListByDynamicGuestListItemDto>>();

        CreateMap<Guest, GetListGuestListItemDto>()
            // 1. Tarihleri Booking içindeki Period Value Object'inden alýyoruz
            .ForMember(dest => dest.CheckInDate, opt => opt.MapFrom(src => src.Booking.Period.Start))
            .ForMember(dest => dest.CheckOutDate, opt => opt.MapFrom(src => src.Booking.Period.End))

            // 2. Oda Adýný Booking -> RoomType üzerinden alýyoruz
            .ForMember(dest => dest.RoomTypeName, opt => opt.MapFrom(src => src.Booking.RoomType.Name))

            // 3. Otel Bilgilerini Booking -> RoomType -> Hotel üzerinden alýyoruz
            .ForMember(dest => dest.HotelName, opt => opt.MapFrom(src => src.Booking.RoomType.Hotel.Name))
            .ForMember(dest => dest.HotelCity, opt => opt.MapFrom(src => src.Booking.RoomType.Hotel.City));
    }
}