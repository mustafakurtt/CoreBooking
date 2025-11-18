using Application.Features.Hotels.Commands.Create;
using Application.Features.Hotels.Commands.Delete;
using Application.Features.Hotels.Commands.Update;
using Application.Features.Hotels.Queries.GetById;
using Application.Features.Hotels.Queries.GetList;
using Application.Features.Hotels.Queries.GetListByDynamic;
using AutoMapper;
using NArchitecture.Core.Application.Responses;
using Domain.Entities;
using NArchitecture.Core.Persistence.Paging;

namespace Application.Features.Hotels.Profiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<CreateHotelCommand, Hotel>();
        CreateMap<Hotel, CreatedHotelResponse>()
            .ForMember(dest => dest.AddressStreet, opt => opt.MapFrom(src => src.Address.Street))
            .ForMember(dest => dest.AddressCity, opt => opt.MapFrom(src => src.Address.City))
            .ForMember(dest => dest.AddressCountry, opt => opt.MapFrom(src => src.Address.Country))
            .ForMember(dest => dest.AddressZipCode, opt => opt.MapFrom(src => src.Address.ZipCode));

        CreateMap<UpdateHotelCommand, Hotel>();
        CreateMap<Hotel, UpdatedHotelResponse>()
            .ForMember(dest => dest.AddressStreet, opt => opt.MapFrom(src => src.Address.Street))
            .ForMember(dest => dest.AddressCity, opt => opt.MapFrom(src => src.Address.City))
            .ForMember(dest => dest.AddressCountry, opt => opt.MapFrom(src => src.Address.Country))
            .ForMember(dest => dest.AddressZipCode, opt => opt.MapFrom(src => src.Address.ZipCode));

        CreateMap<DeleteHotelCommand, Hotel>();
        CreateMap<Hotel, DeletedHotelResponse>();


        CreateMap<Hotel, GetByIdHotelResponse>()
            .ForMember(dest => dest.AddressStreet, opt => opt.MapFrom(src => src.Address.Street))
            .ForMember(dest => dest.AddressCity, opt => opt.MapFrom(src => src.Address.City))
            .ForMember(dest => dest.AddressCountry, opt => opt.MapFrom(src => src.Address.Country))
            .ForMember(dest => dest.AddressZipCode, opt => opt.MapFrom(src => src.Address.ZipCode))
            .ForMember(dest => dest.RoomTypeNames, opt => opt.MapFrom(src => src.RoomTypes.Select(rt => rt.Name).ToList()));

        CreateMap<Hotel, GetListHotelListItemDto>()
            .ForMember(dest => dest.AddressStreet, opt => opt.MapFrom(src => src.Address.Street))
            .ForMember(dest => dest.AddressCity, opt => opt.MapFrom(src => src.Address.City))
            .ForMember(dest => dest.AddressCountry, opt => opt.MapFrom(src => src.Address.Country))
            .ForMember(dest => dest.AddressZipCode, opt => opt.MapFrom(src => src.Address.ZipCode))
            .ForMember(dest => dest.RoomTypeCount, opt => opt.MapFrom(src => src.RoomTypes.Count));

        CreateMap<IPaginate<Hotel>, GetListResponse<GetListHotelListItemDto>>();

        CreateMap<Hotel, GetListByDynamicHotelListItemDto>()
            .ForMember(dest => dest.AddressStreet, opt => opt.MapFrom(src => src.Address.Street))
            .ForMember(dest => dest.AddressCity, opt => opt.MapFrom(src => src.Address.City))
            .ForMember(dest => dest.AddressCountry, opt => opt.MapFrom(src => src.Address.Country))
            .ForMember(dest => dest.AddressZipCode, opt => opt.MapFrom(src => src.Address.ZipCode));

        CreateMap<IPaginate<Hotel>, GetListResponse<GetListByDynamicHotelListItemDto>>();
    }
}