using Application.Features.RoomTypes.Commands.Create;
using Application.Features.RoomTypes.Commands.Delete;
using Application.Features.RoomTypes.Commands.Update;
using Application.Features.RoomTypes.Queries.GetById;
using Application.Features.RoomTypes.Queries.GetList;
using Application.Features.RoomTypes.Queries.GetListByDynamic;
using NArchitecture.Core.Persistence.Dynamic;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Application.Responses;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RoomTypesController : BaseController
{
    [HttpPost]
    public async Task<ActionResult<CreatedRoomTypeResponse>> Add([FromBody] CreateRoomTypeCommand command)
    {
        CreatedRoomTypeResponse response = await Mediator.Send(command);

        return CreatedAtAction(nameof(GetById), new { response.Id }, response);
    }

    [HttpPut]
    public async Task<ActionResult<UpdatedRoomTypeResponse>> Update([FromBody] UpdateRoomTypeCommand command)
    {
        UpdatedRoomTypeResponse response = await Mediator.Send(command);

        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<DeletedRoomTypeResponse>> Delete([FromRoute] Guid id)
    {
        DeleteRoomTypeCommand command = new() { Id = id };

        DeletedRoomTypeResponse response = await Mediator.Send(command);

        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GetByIdRoomTypeResponse>> GetById([FromRoute] Guid id)
    {
        GetByIdRoomTypeQuery query = new() { Id = id };

        GetByIdRoomTypeResponse response = await Mediator.Send(query);

        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<GetListResponse<GetListRoomTypeListItemDto>>> GetList([FromQuery] PageRequest pageRequest)
    {
        GetListRoomTypeQuery query = new() { PageRequest = pageRequest };

        GetListResponse<GetListRoomTypeListItemDto> response = await Mediator.Send(query);

        return Ok(response);
    }

    [HttpPost("GetListByDynamic")]
    public async Task<IActionResult> GetListByDynamic([FromQuery] PageRequest pageRequest, [FromBody] DynamicQuery dynamic)
    {
        GetListByDynamicRoomTypeQuery getListByDynamicRoomTypeQuery = new() { PageRequest = pageRequest, Dynamic = dynamic };
        GetListResponse<GetListByDynamicRoomTypeListItemDto> response = await Mediator.Send(getListByDynamicRoomTypeQuery);
        return Ok(response);
    }
}