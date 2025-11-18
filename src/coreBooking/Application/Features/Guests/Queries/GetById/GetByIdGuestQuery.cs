using Application.Features.Guests.Constants;
using Application.Features.Guests.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using NArchitecture.Core.Application.Pipelines.Authorization;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static Application.Features.Guests.Constants.GuestsOperationClaims;

namespace Application.Features.Guests.Queries.GetById;

public class GetByIdGuestQuery : IRequest<GetByIdGuestResponse>, ISecuredRequest
{
    public Guid Id { get; set; }

    public string[] Roles => [Admin, Read];

    public class GetByIdGuestQueryHandler : IRequestHandler<GetByIdGuestQuery, GetByIdGuestResponse>
    {
        private readonly IMapper _mapper;
        private readonly IGuestRepository _guestRepository;
        private readonly GuestBusinessRules _guestBusinessRules;

        public GetByIdGuestQueryHandler(IMapper mapper, IGuestRepository guestRepository, GuestBusinessRules guestBusinessRules)
        {
            _mapper = mapper;
            _guestRepository = guestRepository;
            _guestBusinessRules = guestBusinessRules;
        }

        public async Task<GetByIdGuestResponse> Handle(GetByIdGuestQuery request, CancellationToken cancellationToken)
        {
            Guest? guest = await _guestRepository.GetAsync(
                predicate: g => g.Id == request.Id,
                include: g => g.Include(x => x.Booking)
                    .ThenInclude(b => b.RoomType)
                    .ThenInclude(rt => rt.Hotel),

                cancellationToken: cancellationToken
            );

            await _guestBusinessRules.GuestShouldExistWhenSelected(guest);

            GetByIdGuestResponse response = _mapper.Map<GetByIdGuestResponse>(guest);
            return response;
        }
    }
}