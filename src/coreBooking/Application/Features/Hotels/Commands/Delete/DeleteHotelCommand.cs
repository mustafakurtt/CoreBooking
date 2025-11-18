using Application.Features.Hotels.Constants;
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

namespace Application.Features.Hotels.Commands.Delete;

public class DeleteHotelCommand : IRequest<DeletedHotelResponse>, ISecuredRequest, ICacheRemoverRequest, ILoggableRequest, ITransactionalRequest
{
    public Guid Id { get; set; }

    public string[] Roles => [Admin, Write, HotelsOperationClaims.Delete];

    public bool BypassCache { get; }
    public string? CacheKey { get; }
    public string[]? CacheGroupKey => ["GetHotels"];

    public class DeleteHotelCommandHandler : IRequestHandler<DeleteHotelCommand, DeletedHotelResponse>
    {
        private readonly IMapper _mapper;
        private readonly IHotelRepository _hotelRepository;
        private readonly HotelBusinessRules _hotelBusinessRules;

        public DeleteHotelCommandHandler(IMapper mapper, IHotelRepository hotelRepository,
                                         HotelBusinessRules hotelBusinessRules)
        {
            _mapper = mapper;
            _hotelRepository = hotelRepository;
            _hotelBusinessRules = hotelBusinessRules;
        }

        public async Task<DeletedHotelResponse> Handle(DeleteHotelCommand request, CancellationToken cancellationToken)
        {
            Hotel? hotel = await _hotelRepository.GetAsync(predicate: h => h.Id == request.Id, cancellationToken: cancellationToken);
            await _hotelBusinessRules.HotelShouldExistWhenSelected(hotel);

            await _hotelRepository.DeleteAsync(hotel!);

            DeletedHotelResponse response = _mapper.Map<DeletedHotelResponse>(hotel);
            return response;
        }
    }
}