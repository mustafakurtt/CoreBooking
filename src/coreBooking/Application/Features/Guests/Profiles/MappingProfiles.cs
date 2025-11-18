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

        CreateMap<Guest, GetByIdGuestResponse>();

        CreateMap<Guest, GetListGuestListItemDto>();
        CreateMap<IPaginate<Guest>, GetListResponse<GetListGuestListItemDto>>();

        CreateMap<Guest, GetListByDynamicGuestListItemDto>();
        CreateMap<IPaginate<Guest>, GetListResponse<GetListByDynamicGuestListItemDto>>();
    }
}