using Application.Features.Inventories.Commands.Create;
using Application.Features.Inventories.Commands.Delete;
using Application.Features.Inventories.Commands.Update;
using Application.Features.Inventories.Queries.GetById;
using Application.Features.Inventories.Queries.GetList;
using Application.Features.Inventories.Queries.GetListByDynamic;
using AutoMapper;
using NArchitecture.Core.Application.Responses;
using Domain.Entities;
using NArchitecture.Core.Persistence.Paging;

namespace Application.Features.Inventories.Profiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {

        CreateMap<CreateInventoryCommand, Inventory>();
        CreateMap<Inventory, CreatedInventoryResponse>()
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price.Amount))
            .ForMember(dest => dest.PriceCurrency, opt => opt.MapFrom(src => src.Price.Currency));

        CreateMap<UpdateInventoryCommand, Inventory>();
        CreateMap<Inventory, UpdatedInventoryResponse>()
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price.Amount))
            .ForMember(dest => dest.PriceCurrency, opt => opt.MapFrom(src => src.Price.Currency));

        CreateMap<DeleteInventoryCommand, Inventory>();
        CreateMap<Inventory, DeletedInventoryResponse>();

        CreateMap<Inventory, GetByIdInventoryResponse>()
            // Fiyat Dönüþümü (Money -> Decimal)
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price.Amount))
            .ForMember(dest => dest.PriceCurrency, opt => opt.MapFrom(src => src.Price.Currency))

            // Ýliþkili Veri Dönüþümü (Flattening)
            .ForMember(dest => dest.RoomTypeName, opt => opt.MapFrom(src => src.RoomType.Name))
            .ForMember(dest => dest.HotelName, opt => opt.MapFrom(src => src.RoomType.Hotel.Name));

        CreateMap<Inventory, GetListInventoryListItemDto>()
            // Fiyat Dönüþümü
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price.Amount))
            .ForMember(dest => dest.PriceCurrency, opt => opt.MapFrom(src => src.Price.Currency))

            // Ýliþkili Veri Dönüþümü
            .ForMember(dest => dest.RoomTypeName, opt => opt.MapFrom(src => src.RoomType.Name))
            .ForMember(dest => dest.HotelName, opt => opt.MapFrom(src => src.RoomType.Hotel.Name));

        CreateMap<IPaginate<Inventory>, GetListResponse<GetListInventoryListItemDto>>();

        CreateMap<Inventory, GetListByDynamicInventoryListItemDto>()
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price.Amount));

        CreateMap<Inventory, GetListByDynamicInventoryListItemDto>()
            // Fiyat Dönüþümü
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price.Amount))
            .ForMember(dest => dest.PriceCurrency, opt => opt.MapFrom(src => src.Price.Currency))

            // Ýliþkili Veri Dönüþümü
            .ForMember(dest => dest.RoomTypeName, opt => opt.MapFrom(src => src.RoomType.Name))
            .ForMember(dest => dest.HotelName, opt => opt.MapFrom(src => src.RoomType.Hotel.Name));
    }
}