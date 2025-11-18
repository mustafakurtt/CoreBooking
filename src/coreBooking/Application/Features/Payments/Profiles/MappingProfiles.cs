using Application.Features.Payments.Commands.Create;
using Application.Features.Payments.Commands.Delete;
using Application.Features.Payments.Commands.Update;
using Application.Features.Payments.Queries.GetById;
using Application.Features.Payments.Queries.GetList;
using Application.Features.Payments.Queries.GetListByDynamic;
using AutoMapper;
using NArchitecture.Core.Application.Responses;
using Domain.Entities;
using NArchitecture.Core.Persistence.Paging;

namespace Application.Features.Payments.Profiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<CreatePaymentCommand, Payment>();
        CreateMap<Payment, CreatedPaymentResponse>();

        CreateMap<UpdatePaymentCommand, Payment>();
        CreateMap<Payment, UpdatedPaymentResponse>();

        CreateMap<DeletePaymentCommand, Payment>();
        CreateMap<Payment, DeletedPaymentResponse>();

        CreateMap<Payment, GetByIdPaymentResponse>();

        CreateMap<Payment, GetListPaymentListItemDto>();
        CreateMap<IPaginate<Payment>, GetListResponse<GetListPaymentListItemDto>>();

        CreateMap<Payment, GetListByDynamicPaymentListItemDto>();
        CreateMap<IPaginate<Payment>, GetListResponse<GetListByDynamicPaymentListItemDto>>();

        CreateMap<Payment, CreatedPaymentResponse>()
            .ForMember(dest => dest.AmountValue, opt => opt.MapFrom(src => src.Amount.Amount))
            .ForMember(dest => dest.AmountCurrency, opt => opt.MapFrom(src => src.Amount.Currency));



    }
}