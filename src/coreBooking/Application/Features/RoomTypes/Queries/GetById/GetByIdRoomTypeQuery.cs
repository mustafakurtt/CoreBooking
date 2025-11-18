using Application.Features.RoomTypes.Constants;
using Application.Features.RoomTypes.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using NArchitecture.Core.Application.Pipelines.Authorization;
using MediatR;
using static Application.Features.RoomTypes.Constants.RoomTypesOperationClaims;

namespace Application.Features.RoomTypes.Queries.GetById;

public class GetByIdRoomTypeQuery : IRequest<GetByIdRoomTypeResponse>, ISecuredRequest
{
    public Guid Id { get; set; }

    public string[] Roles => [Admin, Read];

    public class GetByIdRoomTypeQueryHandler : IRequestHandler<GetByIdRoomTypeQuery, GetByIdRoomTypeResponse>
    {
        private readonly IMapper _mapper;
        private readonly IRoomTypeRepository _roomTypeRepository;
        private readonly RoomTypeBusinessRules _roomTypeBusinessRules;

        public GetByIdRoomTypeQueryHandler(IMapper mapper, IRoomTypeRepository roomTypeRepository, RoomTypeBusinessRules roomTypeBusinessRules)
        {
            _mapper = mapper;
            _roomTypeRepository = roomTypeRepository;
            _roomTypeBusinessRules = roomTypeBusinessRules;
        }

        public async Task<GetByIdRoomTypeResponse> Handle(GetByIdRoomTypeQuery request, CancellationToken cancellationToken)
        {
            RoomType? roomType = await _roomTypeRepository.GetAsync(predicate: rt => rt.Id == request.Id, cancellationToken: cancellationToken);
            await _roomTypeBusinessRules.RoomTypeShouldExistWhenSelected(roomType);

            GetByIdRoomTypeResponse response = _mapper.Map<GetByIdRoomTypeResponse>(roomType);
            return response;
        }
    }
}