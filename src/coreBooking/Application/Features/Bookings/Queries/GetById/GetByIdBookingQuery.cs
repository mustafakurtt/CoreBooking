using Application.Features.Bookings.Constants;
using Application.Features.Bookings.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using NArchitecture.Core.Application.Pipelines.Authorization;
using MediatR;
using static Application.Features.Bookings.Constants.BookingsOperationClaims;

namespace Application.Features.Bookings.Queries.GetById;

public class GetByIdBookingQuery : IRequest<GetByIdBookingResponse>, ISecuredRequest
{
    public Guid Id { get; set; }

    public string[] Roles => [Admin, Read];

    public class GetByIdBookingQueryHandler : IRequestHandler<GetByIdBookingQuery, GetByIdBookingResponse>
    {
        private readonly IMapper _mapper;
        private readonly IBookingRepository _bookingRepository;
        private readonly BookingBusinessRules _bookingBusinessRules;

        public GetByIdBookingQueryHandler(IMapper mapper, IBookingRepository bookingRepository, BookingBusinessRules bookingBusinessRules)
        {
            _mapper = mapper;
            _bookingRepository = bookingRepository;
            _bookingBusinessRules = bookingBusinessRules;
        }

        public async Task<GetByIdBookingResponse> Handle(GetByIdBookingQuery request, CancellationToken cancellationToken)
        {
            Booking? booking = await _bookingRepository.GetAsync(predicate: b => b.Id == request.Id, cancellationToken: cancellationToken);
            await _bookingBusinessRules.BookingShouldExistWhenSelected(booking);

            GetByIdBookingResponse response = _mapper.Map<GetByIdBookingResponse>(booking);
            return response;
        }
    }
}