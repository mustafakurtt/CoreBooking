using Application.Features.RoomTypes.Constants;
using Application.Features.RoomTypes.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using NArchitecture.Core.Application.Pipelines.Authorization;
using NArchitecture.Core.Application.Pipelines.Caching;
using NArchitecture.Core.Application.Pipelines.Logging;
using NArchitecture.Core.Application.Pipelines.Transaction;
using MediatR;
using static Application.Features.RoomTypes.Constants.RoomTypesOperationClaims;

namespace Application.Features.RoomTypes.Commands.Create;

public class CreateRoomTypeCommand : IRequest<CreatedRoomTypeResponse>, ISecuredRequest, ILoggableRequest, ITransactionalRequest
{
    public required Guid HotelId { get; set; }
    public required string Name { get; set; }
    public required int Capacity { get; set; }

    public string[] Roles => [Admin, Write, RoomTypesOperationClaims.Create];

    public class CreateRoomTypeCommandHandler : IRequestHandler<CreateRoomTypeCommand, CreatedRoomTypeResponse>
    {
        private readonly IMapper _mapper;
        private readonly IRoomTypeRepository _roomTypeRepository;
        private readonly RoomTypeBusinessRules _roomTypeBusinessRules;

        public CreateRoomTypeCommandHandler(IMapper mapper, IRoomTypeRepository roomTypeRepository,
            RoomTypeBusinessRules roomTypeBusinessRules)
        {
            _mapper = mapper;
            _roomTypeRepository = roomTypeRepository;
            _roomTypeBusinessRules = roomTypeBusinessRules;
        }

        public async Task<CreatedRoomTypeResponse> Handle(CreateRoomTypeCommand request, CancellationToken cancellationToken)
        {
            // KURAL: Otel var mý?
            await _roomTypeBusinessRules.HotelIdShouldExist(request.HotelId, cancellationToken);

            RoomType roomType = _mapper.Map<RoomType>(request);
            await _roomTypeRepository.AddAsync(roomType);

            CreatedRoomTypeResponse response = _mapper.Map<CreatedRoomTypeResponse>(roomType);
            return response;
        }
    }
}