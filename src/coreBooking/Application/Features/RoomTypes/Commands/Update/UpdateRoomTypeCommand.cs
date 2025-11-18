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

namespace Application.Features.RoomTypes.Commands.Update;

public class UpdateRoomTypeCommand : IRequest<UpdatedRoomTypeResponse>, ISecuredRequest, ICacheRemoverRequest, ILoggableRequest, ITransactionalRequest
{
    public Guid Id { get; set; }
    public required Guid HotelId { get; set; }
    public required string Name { get; set; }
    public required int Capacity { get; set; }

    public string[] Roles => [Admin, Write, RoomTypesOperationClaims.Update];

    public bool BypassCache { get; }
    public string? CacheKey { get; }
    public string[]? CacheGroupKey => ["GetRoomTypes"];

    public class UpdateRoomTypeCommandHandler : IRequestHandler<UpdateRoomTypeCommand, UpdatedRoomTypeResponse>
    {
        private readonly IMapper _mapper;
        private readonly IRoomTypeRepository _roomTypeRepository;
        private readonly RoomTypeBusinessRules _roomTypeBusinessRules;

        public UpdateRoomTypeCommandHandler(IMapper mapper, IRoomTypeRepository roomTypeRepository,
                                         RoomTypeBusinessRules roomTypeBusinessRules)
        {
            _mapper = mapper;
            _roomTypeRepository = roomTypeRepository;
            _roomTypeBusinessRules = roomTypeBusinessRules;
        }

        public async Task<UpdatedRoomTypeResponse> Handle(UpdateRoomTypeCommand request, CancellationToken cancellationToken)
        {
            RoomType? roomType = await _roomTypeRepository.GetAsync(predicate: rt => rt.Id == request.Id, cancellationToken: cancellationToken);
            await _roomTypeBusinessRules.RoomTypeShouldExistWhenSelected(roomType);
            roomType = _mapper.Map(request, roomType);

            await _roomTypeRepository.UpdateAsync(roomType!);

            UpdatedRoomTypeResponse response = _mapper.Map<UpdatedRoomTypeResponse>(roomType);
            return response;
        }
    }
}