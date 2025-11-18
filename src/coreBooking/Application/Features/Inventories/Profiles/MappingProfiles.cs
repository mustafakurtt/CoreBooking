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
        CreateMap<Inventory, CreatedInventoryResponse>();

        CreateMap<UpdateInventoryCommand, Inventory>();
        CreateMap<Inventory, UpdatedInventoryResponse>();

        CreateMap<DeleteInventoryCommand, Inventory>();
        CreateMap<Inventory, DeletedInventoryResponse>();

        CreateMap<Inventory, GetByIdInventoryResponse>();

        CreateMap<Inventory, GetListInventoryListItemDto>();
        CreateMap<IPaginate<Inventory>, GetListResponse<GetListInventoryListItemDto>>();

        CreateMap<Inventory, GetListByDynamicInventoryListItemDto>();
        CreateMap<IPaginate<Inventory>, GetListResponse<GetListByDynamicInventoryListItemDto>>();
    }
}