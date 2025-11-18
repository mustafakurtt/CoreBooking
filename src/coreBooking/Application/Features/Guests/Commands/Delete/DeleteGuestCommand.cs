using Application.Features.Guests.Constants;
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

namespace Application.Features.Guests.Commands.Delete;

public class DeleteGuestCommand : IRequest<DeletedGuestResponse>, ISecuredRequest, ILoggableRequest, ITransactionalRequest
{
    public Guid Id { get; set; }

    public string[] Roles => [Admin, Write, GuestsOperationClaims.Delete];

    public class DeleteGuestCommandHandler : IRequestHandler<DeleteGuestCommand, DeletedGuestResponse>
    {
        private readonly IMapper _mapper;
        private readonly IGuestRepository _guestRepository;
        private readonly GuestBusinessRules _guestBusinessRules;

        public DeleteGuestCommandHandler(IMapper mapper, IGuestRepository guestRepository,
                                         GuestBusinessRules guestBusinessRules)
        {
            _mapper = mapper;
            _guestRepository = guestRepository;
            _guestBusinessRules = guestBusinessRules;
        }

        public async Task<DeletedGuestResponse> Handle(DeleteGuestCommand request, CancellationToken cancellationToken)
        {
            Guest? guest = await _guestRepository.GetAsync(predicate: g => g.Id == request.Id, cancellationToken: cancellationToken);
            await _guestBusinessRules.GuestShouldExistWhenSelected(guest);

            await _guestRepository.DeleteAsync(guest!);

            DeletedGuestResponse response = _mapper.Map<DeletedGuestResponse>(guest);
            return response;
        }
    }
}