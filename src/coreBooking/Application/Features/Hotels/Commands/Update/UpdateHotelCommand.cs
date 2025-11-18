using Application.Features.Hotels.Constants;
using Application.Features.Hotels.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using NArchitecture.Core.Application.Pipelines.Authorization;
using NArchitecture.Core.Application.Pipelines.Caching;
using NArchitecture.Core.Application.Pipelines.Logging;
using NArchitecture.Core.Application.Pipelines.Transaction;
using MediatR;
using static Application.Features.Hotels.Constants.HotelsOperationClaims;

namespace Application.Features.Hotels.Commands.Update;

public class UpdateHotelCommand : IRequest<UpdatedHotelResponse>, ISecuredRequest, ICacheRemoverRequest, ILoggableRequest, ITransactionalRequest
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string City { get; set; }
    public required string Address { get; set; }

    public string[] Roles => [Admin, Write, HotelsOperationClaims.Update];

    public bool BypassCache { get; }
    public string? CacheKey { get; }
    public string[]? CacheGroupKey => ["GetHotels"];

    public class UpdateHotelCommandHandler : IRequestHandler<UpdateHotelCommand, UpdatedHotelResponse>
    {
        private readonly IMapper _mapper;
        private readonly IHotelRepository _hotelRepository;
        private readonly HotelBusinessRules _hotelBusinessRules;

        public UpdateHotelCommandHandler(IMapper mapper, IHotelRepository hotelRepository,
                                         HotelBusinessRules hotelBusinessRules)
        {
            _mapper = mapper;
            _hotelRepository = hotelRepository;
            _hotelBusinessRules = hotelBusinessRules;
        }

        public async Task<UpdatedHotelResponse> Handle(UpdateHotelCommand request, CancellationToken cancellationToken)
        {
            Hotel? hotel = await _hotelRepository.GetAsync(predicate: h => h.Id == request.Id, cancellationToken: cancellationToken);
            await _hotelBusinessRules.HotelShouldExistWhenSelected(hotel);
            hotel = _mapper.Map(request, hotel);

            await _hotelRepository.UpdateAsync(hotel!);

            UpdatedHotelResponse response = _mapper.Map<UpdatedHotelResponse>(hotel);
            return response;
        }
    }
}