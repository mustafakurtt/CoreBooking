using Application.Features.RoomTypes.Constants;
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

namespace Application.Features.RoomTypes.Commands.Delete;

public class DeleteRoomTypeCommand : IRequest<DeletedRoomTypeResponse>, ISecuredRequest, ICacheRemoverRequest, ILoggableRequest, ITransactionalRequest
{
    public Guid Id { get; set; }

    public string[] Roles => [Admin, Write, RoomTypesOperationClaims.Delete];

    public bool BypassCache { get; }
    public string? CacheKey { get; }
    public string[]? CacheGroupKey => ["GetRoomTypes"];

    public class DeleteRoomTypeCommandHandler : IRequestHandler<DeleteRoomTypeCommand, DeletedRoomTypeResponse>
    {
        private readonly IMapper _mapper;
        private readonly IRoomTypeRepository _roomTypeRepository;
        private readonly RoomTypeBusinessRules _roomTypeBusinessRules;

        public DeleteRoomTypeCommandHandler(IMapper mapper, IRoomTypeRepository roomTypeRepository,
                                         RoomTypeBusinessRules roomTypeBusinessRules)
        {
            _mapper = mapper;
            _roomTypeRepository = roomTypeRepository;
            _roomTypeBusinessRules = roomTypeBusinessRules;
        }

        public async Task<DeletedRoomTypeResponse> Handle(DeleteRoomTypeCommand request, CancellationToken cancellationToken)
        {
            RoomType? roomType = await _roomTypeRepository.GetAsync(predicate: rt => rt.Id == request.Id, cancellationToken: cancellationToken);
            await _roomTypeBusinessRules.RoomTypeShouldExistWhenSelected(roomType);

            await _roomTypeRepository.DeleteAsync(roomType!);

            DeletedRoomTypeResponse response = _mapper.Map<DeletedRoomTypeResponse>(roomType);
            return response;
        }
    }
}