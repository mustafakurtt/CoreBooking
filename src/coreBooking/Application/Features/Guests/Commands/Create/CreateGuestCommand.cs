using Application.Features.Guests.Constants;
using Application.Features.Guests.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using NArchitecture.Core.Application.Pipelines.Authorization;
using NArchitecture.Core.Application.Pipelines.Logging;
using NArchitecture.Core.Application.Pipelines.Transaction;
using MediatR;
using static Application.Features.Guests.Constants.GuestsOperationClaims;

namespace Application.Features.Guests.Commands.Create;

public class CreateGuestCommand : IRequest<CreatedGuestResponse>, ISecuredRequest, ILoggableRequest, ITransactionalRequest
{
    public required Guid BookingId { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required DateTime DateOfBirth { get; set; }
    public required string Nationality { get; set; }
    public required bool IsPrimary { get; set; }

    public string[] Roles => [Admin, Write, GuestsOperationClaims.Create];

    public class CreateGuestCommandHandler : IRequestHandler<CreateGuestCommand, CreatedGuestResponse>
    {
        private readonly IMapper _mapper;
        private readonly IGuestRepository _guestRepository;
        private readonly GuestBusinessRules _guestBusinessRules;

        public CreateGuestCommandHandler(IMapper mapper, IGuestRepository guestRepository,
                                         GuestBusinessRules guestBusinessRules)
        {
            _mapper = mapper;
            _guestRepository = guestRepository;
            _guestBusinessRules = guestBusinessRules;
        }

        public async Task<CreatedGuestResponse> Handle(CreateGuestCommand request, CancellationToken cancellationToken)
        {
            Guest guest = _mapper.Map<Guest>(request);

            await _guestRepository.AddAsync(guest);

            CreatedGuestResponse response = _mapper.Map<CreatedGuestResponse>(guest);
            return response;
        }
    }
}