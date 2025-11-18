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
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price.Amount)); // Money.Amount -> decimal

        CreateMap<UpdateInventoryCommand, Inventory>();
        CreateMap<Inventory, UpdatedInventoryResponse>()
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price.Amount)); // Update dönüþü için de gerekli

        CreateMap<DeleteInventoryCommand, Inventory>();
        CreateMap<Inventory, DeletedInventoryResponse>();

        CreateMap<Inventory, GetByIdInventoryResponse>()
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price.Amount));

        CreateMap<Inventory, GetListInventoryListItemDto>()
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price.Amount));

        CreateMap<IPaginate<Inventory>, GetListResponse<GetListInventoryListItemDto>>();

        CreateMap<Inventory, GetListByDynamicInventoryListItemDto>()
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price.Amount));

        CreateMap<IPaginate<Inventory>, GetListResponse<GetListByDynamicInventoryListItemDto>>();
    }
}