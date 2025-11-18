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
        CreateMap<Hotel, CreatedHotelResponse>();

        CreateMap<UpdateHotelCommand, Hotel>();
        CreateMap<Hotel, UpdatedHotelResponse>();

        CreateMap<DeleteHotelCommand, Hotel>();
        CreateMap<Hotel, DeletedHotelResponse>();

        CreateMap<Hotel, GetByIdHotelResponse>();

        CreateMap<Hotel, GetListHotelListItemDto>();
        CreateMap<IPaginate<Hotel>, GetListResponse<GetListHotelListItemDto>>();

        CreateMap<Hotel, GetListByDynamicHotelListItemDto>();
        CreateMap<IPaginate<Hotel>, GetListResponse<GetListByDynamicHotelListItemDto>>();
    }
}