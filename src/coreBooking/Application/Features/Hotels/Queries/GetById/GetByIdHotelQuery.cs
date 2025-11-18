using Application.Features.Hotels.Constants;
using Application.Features.Hotels.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using NArchitecture.Core.Application.Pipelines.Authorization;
using MediatR;
using static Application.Features.Hotels.Constants.HotelsOperationClaims;

namespace Application.Features.Hotels.Queries.GetById;

public class GetByIdHotelQuery : IRequest<GetByIdHotelResponse>, ISecuredRequest
{
    public Guid Id { get; set; }

    public string[] Roles => [Admin, Read];

    public class GetByIdHotelQueryHandler : IRequestHandler<GetByIdHotelQuery, GetByIdHotelResponse>
    {
        private readonly IMapper _mapper;
        private readonly IHotelRepository _hotelRepository;
        private readonly HotelBusinessRules _hotelBusinessRules;

        public GetByIdHotelQueryHandler(IMapper mapper, IHotelRepository hotelRepository, HotelBusinessRules hotelBusinessRules)
        {
            _mapper = mapper;
            _hotelRepository = hotelRepository;
            _hotelBusinessRules = hotelBusinessRules;
        }

        public async Task<GetByIdHotelResponse> Handle(GetByIdHotelQuery request, CancellationToken cancellationToken)
        {
            Hotel? hotel = await _hotelRepository.GetAsync(predicate: h => h.Id == request.Id, cancellationToken: cancellationToken);
            await _hotelBusinessRules.HotelShouldExistWhenSelected(hotel);

            GetByIdHotelResponse response = _mapper.Map<GetByIdHotelResponse>(hotel);
            return response;
        }
    }
}