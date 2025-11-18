using Application.Features.Bookings.Constants;
using Application.Features.Bookings.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using NArchitecture.Core.Application.Pipelines.Authorization;
using NArchitecture.Core.Application.Pipelines.Logging;
using NArchitecture.Core.Application.Pipelines.Transaction;
using MediatR;
using Shared.Enums;
using static Application.Features.Bookings.Constants.BookingsOperationClaims;

namespace Application.Features.Bookings.Commands.Update;

public class UpdateBookingCommand : IRequest<UpdatedBookingResponse>, ISecuredRequest, ILoggableRequest, ITransactionalRequest
{
    public Guid Id { get; set; }
    public required Guid CustomerId { get; set; }
    public required Guid RoomTypeId { get; set; }
    public required DateTime CheckInDate { get; set; }
    public required DateTime CheckOutDate { get; set; }
    public required int NumberOfGuests { get; set; }
    public required decimal TotalPrice { get; set; }
    public required BookingStatus Status { get; set; }

    public string[] Roles => [Admin, Write, BookingsOperationClaims.Update];

    public class UpdateBookingCommandHandler : IRequestHandler<UpdateBookingCommand, UpdatedBookingResponse>
    {
        private readonly IMapper _mapper;
        private readonly IBookingRepository _bookingRepository;
        private readonly BookingBusinessRules _bookingBusinessRules;

        public UpdateBookingCommandHandler(IMapper mapper, IBookingRepository bookingRepository,
                                         BookingBusinessRules bookingBusinessRules)
        {
            _mapper = mapper;
            _bookingRepository = bookingRepository;
            _bookingBusinessRules = bookingBusinessRules;
        }

        public async Task<UpdatedBookingResponse> Handle(UpdateBookingCommand request, CancellationToken cancellationToken)
        {
            Booking? booking = await _bookingRepository.GetAsync(predicate: b => b.Id == request.Id, cancellationToken: cancellationToken);
            await _bookingBusinessRules.BookingShouldExistWhenSelected(booking);
            booking = _mapper.Map(request, booking);

            await _bookingRepository.UpdateAsync(booking!);

            UpdatedBookingResponse response = _mapper.Map<UpdatedBookingResponse>(booking);
            return response;
        }
    }
}