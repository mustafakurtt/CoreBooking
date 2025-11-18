using Application.Features.Hotels.Constants;
using Application.Features.Hotels.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using Domain.ValueObjects;
using MediatR;
using NArchitecture.Core.Application.Pipelines.Authorization;
using NArchitecture.Core.Application.Pipelines.Caching;
using NArchitecture.Core.Application.Pipelines.Logging;
using NArchitecture.Core.Application.Pipelines.Transaction;
using static Application.Features.Hotels.Constants.HotelsOperationClaims;

namespace Application.Features.Hotels.Commands.Update;

public class UpdateHotelCommand : IRequest<UpdatedHotelResponse>, ISecuredRequest, ICacheRemoverRequest, ILoggableRequest, ITransactionalRequest
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string City { get; set; }

    // --- V2: Flat Address Inputs ---
    public required string AddressStreet { get; set; }
    public required string AddressCity { get; set; }
    public required string AddressCountry { get; set; }
    public required string AddressZipCode { get; set; }

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

            if (hotel!.Name != request.Name)
            {
                await _hotelBusinessRules.HotelNameShouldNotExistsWhenUpdate(request.Id, request.Name, cancellationToken);
            }

            _mapper.Map(request, hotel);

            hotel.Address = new Address(
                request.AddressStreet,
                request.AddressCity,
                request.AddressCountry,
                request.AddressZipCode
            );

            await _hotelRepository.UpdateAsync(hotel!);

            // 7. Response
            UpdatedHotelResponse response = _mapper.Map<UpdatedHotelResponse>(hotel);
            return response;
        }
    }
}