using Application.Features.Bookings.Constants;
using Application.Features.Bookings.Constants;
using Application.Features.Bookings.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using NArchitecture.Core.Application.Pipelines.Authorization;
using NArchitecture.Core.Application.Pipelines.Logging;
using NArchitecture.Core.Application.Pipelines.Transaction;
using MediatR;
using static Application.Features.Bookings.Constants.BookingsOperationClaims;

namespace Application.Features.Bookings.Commands.Delete;

public class DeleteBookingCommand : IRequest<DeletedBookingResponse>, ISecuredRequest, ILoggableRequest, ITransactionalRequest
{
    public Guid Id { get; set; }

    public string[] Roles => [Admin, Write, BookingsOperationClaims.Delete];

    public class DeleteBookingCommandHandler : IRequestHandler<DeleteBookingCommand, DeletedBookingResponse>
    {
        private readonly IMapper _mapper;
        private readonly IBookingRepository _bookingRepository;
        private readonly BookingBusinessRules _bookingBusinessRules;

        public DeleteBookingCommandHandler(IMapper mapper, IBookingRepository bookingRepository,
                                         BookingBusinessRules bookingBusinessRules)
        {
            _mapper = mapper;
            _bookingRepository = bookingRepository;
            _bookingBusinessRules = bookingBusinessRules;
        }

        public async Task<DeletedBookingResponse> Handle(DeleteBookingCommand request, CancellationToken cancellationToken)
        {
            Booking? booking = await _bookingRepository.GetAsync(predicate: b => b.Id == request.Id, cancellationToken: cancellationToken);
            await _bookingBusinessRules.BookingShouldExistWhenSelected(booking);

            await _bookingRepository.DeleteAsync(booking!);

            DeletedBookingResponse response = _mapper.Map<DeletedBookingResponse>(booking);
            return response;
        }
    }
}