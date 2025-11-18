using Application.Features.Hotels.Commands.Create;
using Application.Features.Hotels.Commands.Delete;
using Application.Features.Hotels.Commands.Update;
using Application.Features.Hotels.Queries.GetById;
using Application.Features.Hotels.Queries.GetList;
using Application.Features.Hotels.Queries.GetListByDynamic;
using NArchitecture.Core.Persistence.Dynamic;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Application.Responses;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HotelsController : BaseController
{
    [HttpPost]
    public async Task<ActionResult<CreatedHotelResponse>> Add([FromBody] CreateHotelCommand command)
    {
        CreatedHotelResponse response = await Mediator.Send(command);

        return CreatedAtAction(nameof(GetById), new { response.Id }, response);
    }

    [HttpPut]
    public async Task<ActionResult<UpdatedHotelResponse>> Update([FromBody] UpdateHotelCommand command)
    {
        UpdatedHotelResponse response = await Mediator.Send(command);

        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<DeletedHotelResponse>> Delete([FromRoute] Guid id)
    {
        DeleteHotelCommand command = new() { Id = id };

        DeletedHotelResponse response = await Mediator.Send(command);

        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GetByIdHotelResponse>> GetById([FromRoute] Guid id)
    {
        GetByIdHotelQuery query = new() { Id = id };

        GetByIdHotelResponse response = await Mediator.Send(query);

        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<GetListResponse<GetListHotelListItemDto>>> GetList([FromQuery] PageRequest pageRequest)
    {
        GetListHotelQuery query = new() { PageRequest = pageRequest };

        GetListResponse<GetListHotelListItemDto> response = await Mediator.Send(query);

        return Ok(response);
    }

    [HttpPost("GetListByDynamic")]
    public async Task<IActionResult> GetListByDynamic([FromQuery] PageRequest pageRequest, [FromBody] DynamicQuery dynamic)
    {
        GetListByDynamicHotelQuery getListByDynamicHotelQuery = new() { PageRequest = pageRequest, Dynamic = dynamic };
        GetListResponse<GetListByDynamicHotelListItemDto> response = await Mediator.Send(getListByDynamicHotelQuery);
        return Ok(response);
    }
}