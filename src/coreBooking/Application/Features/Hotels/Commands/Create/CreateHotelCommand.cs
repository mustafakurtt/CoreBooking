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

namespace Application.Features.Hotels.Commands.Create;

public class CreateHotelCommand : IRequest<CreatedHotelResponse>, ISecuredRequest, ICacheRemoverRequest, ILoggableRequest, ITransactionalRequest
{
    public required string Name { get; set; }
    public required string City { get; set; }
    public required string Address { get; set; }

    public string[] Roles => [Admin, Write, HotelsOperationClaims.Create];

    public bool BypassCache { get; }
    public string? CacheKey { get; }
    public string[]? CacheGroupKey => ["GetHotels"];

    public class CreateHotelCommandHandler : IRequestHandler<CreateHotelCommand, CreatedHotelResponse>
    {
        private readonly IMapper _mapper;
        private readonly IHotelRepository _hotelRepository;
        private readonly HotelBusinessRules _hotelBusinessRules;

        public CreateHotelCommandHandler(IMapper mapper, IHotelRepository hotelRepository,
                                         HotelBusinessRules hotelBusinessRules)
        {
            _mapper = mapper;
            _hotelRepository = hotelRepository;
            _hotelBusinessRules = hotelBusinessRules;
        }

        public async Task<CreatedHotelResponse> Handle(CreateHotelCommand request, CancellationToken cancellationToken)
        {
            Hotel hotel = _mapper.Map<Hotel>(request);

            await _hotelRepository.AddAsync(hotel);

            CreatedHotelResponse response = _mapper.Map<CreatedHotelResponse>(hotel);
            return response;
        }
    }
}