using Application.Features.Bookings.Commands.Cancel;
using Application.Features.Bookings.Commands.Create;
using Application.Features.Bookings.Commands.Delete;
using Application.Features.Bookings.Commands.Update;
using Application.Features.Bookings.Queries.GetById;
using Application.Features.Bookings.Queries.GetList;
using Application.Features.Bookings.Queries.GetListByDynamic;
using AutoMapper;
using Domain.Entities;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Paging;

namespace Application.Features.Bookings.Profiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<CreateBookingCommand, Booking>();
        CreateMap<Booking, CreatedBookingResponse>();

        CreateMap<UpdateBookingCommand, Booking>();
        CreateMap<Booking, UpdatedBookingResponse>();

        CreateMap<DeleteBookingCommand, Booking>();
        CreateMap<Booking, DeletedBookingResponse>();

        CreateMap<Booking, GetByIdBookingResponse>();

        CreateMap<Booking, GetListBookingListItemDto>();
        CreateMap<IPaginate<Booking>, GetListResponse<GetListBookingListItemDto>>();

        CreateMap<Booking, GetListByDynamicBookingListItemDto>();
        CreateMap<IPaginate<Booking>, GetListResponse<GetListByDynamicBookingListItemDto>>();

        CreateMap<Booking, CanceledBookingResponse>();

        CreateMap<Booking, CreatedBookingResponse>()
            .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.TotalPrice.Amount))
            .ForMember(dest => dest.CheckInDate, opt => opt.MapFrom(src => src.Period.Start))
            .ForMember(dest => dest.CheckOutDate, opt => opt.MapFrom(src => src.Period.End));
    }
}