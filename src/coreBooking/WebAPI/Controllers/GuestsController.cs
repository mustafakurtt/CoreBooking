using Application.Features.Guests.Commands.Create;
using Application.Features.Guests.Commands.Delete;
using Application.Features.Guests.Commands.Update;
using Application.Features.Guests.Queries.GetById;
using Application.Features.Guests.Queries.GetList;
using Application.Features.Guests.Queries.GetListByDynamic;
using NArchitecture.Core.Persistence.Dynamic;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Application.Responses;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GuestsController : BaseController
{
    [HttpPost]
    public async Task<ActionResult<CreatedGuestResponse>> Add([FromBody] CreateGuestCommand command)
    {
        CreatedGuestResponse response = await Mediator.Send(command);

        return CreatedAtAction(nameof(GetById), new { response.Id }, response);
    }

    [HttpPut]
    public async Task<ActionResult<UpdatedGuestResponse>> Update([FromBody] UpdateGuestCommand command)
    {
        UpdatedGuestResponse response = await Mediator.Send(command);

        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<DeletedGuestResponse>> Delete([FromRoute] Guid id)
    {
        DeleteGuestCommand command = new() { Id = id };

        DeletedGuestResponse response = await Mediator.Send(command);

        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GetByIdGuestResponse>> GetById([FromRoute] Guid id)
    {
        GetByIdGuestQuery query = new() { Id = id };

        GetByIdGuestResponse response = await Mediator.Send(query);

        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<GetListResponse<GetListGuestListItemDto>>> GetList([FromQuery] PageRequest pageRequest)
    {
        GetListGuestQuery query = new() { PageRequest = pageRequest };

        GetListResponse<GetListGuestListItemDto> response = await Mediator.Send(query);

        return Ok(response);
    }

    [HttpPost("GetListByDynamic")]
    public async Task<IActionResult> GetListByDynamic([FromQuery] PageRequest pageRequest, [FromBody] DynamicQuery dynamic)
    {
        GetListByDynamicGuestQuery getListByDynamicGuestQuery = new() { PageRequest = pageRequest, Dynamic = dynamic };
        GetListResponse<GetListByDynamicGuestListItemDto> response = await Mediator.Send(getListByDynamicGuestQuery);
        return Ok(response);
    }
}