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

namespace Application.Features.Guests.Commands.Update;

public class UpdateGuestCommand : IRequest<UpdatedGuestResponse>, ISecuredRequest, ILoggableRequest, ITransactionalRequest
{
    public Guid Id { get; set; }
    public required Guid BookingId { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required DateTime DateOfBirth { get; set; }
    public required string Nationality { get; set; }
    public required bool IsPrimary { get; set; }

    public string[] Roles => [Admin, Write, GuestsOperationClaims.Update];

    public class UpdateGuestCommandHandler : IRequestHandler<UpdateGuestCommand, UpdatedGuestResponse>
    {
        private readonly IMapper _mapper;
        private readonly IGuestRepository _guestRepository;
        private readonly GuestBusinessRules _guestBusinessRules;

        public UpdateGuestCommandHandler(IMapper mapper, IGuestRepository guestRepository,
                                         GuestBusinessRules guestBusinessRules)
        {
            _mapper = mapper;
            _guestRepository = guestRepository;
            _guestBusinessRules = guestBusinessRules;
        }

        public async Task<UpdatedGuestResponse> Handle(UpdateGuestCommand request, CancellationToken cancellationToken)
        {
            // 1. Misafiri Getir
            Guest? guest = await _guestRepository.GetAsync(
                predicate: g => g.Id == request.Id,
                cancellationToken: cancellationToken
            );

            // 2. Temel Kontrol: Misafir var mý?
            await _guestBusinessRules.GuestShouldExistWhenSelected(guest);

            // 3. Ýliþki Kontrolü: Booking var mý?
            await _guestBusinessRules.BookingIdShouldExist(request.BookingId, cancellationToken);

            // 4. Kritik Kural: Rezervasyon Aktif mi?
            // (Geçmiþ veya iptal edilmiþ rezervasyonun misafiri deðiþtirilemez)
            await _guestBusinessRules.BookingShouldBeActive(request.BookingId, cancellationToken);

            // 5. Mapping ve Güncelleme
            _mapper.Map(request, guest);
            await _guestRepository.UpdateAsync(guest!);

            // 6. Response
            UpdatedGuestResponse response = _mapper.Map<UpdatedGuestResponse>(guest);
            return response;
        }
    }
}