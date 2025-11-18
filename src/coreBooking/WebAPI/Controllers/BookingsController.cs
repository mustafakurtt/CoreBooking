using Application.Features.Bookings.Commands.Create;
using Application.Features.Bookings.Commands.Delete;
using Application.Features.Bookings.Commands.Update;
using Application.Features.Bookings.Queries.GetById;
using Application.Features.Bookings.Queries.GetList;
using Application.Features.Bookings.Queries.GetListByDynamic;
using NArchitecture.Core.Persistence.Dynamic;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Application.Responses;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BookingsController : BaseController
{
    [HttpPost]
    public async Task<ActionResult<CreatedBookingResponse>> Add([FromBody] CreateBookingCommand command)
    {
        CreatedBookingResponse response = await Mediator.Send(command);

        return CreatedAtAction(nameof(GetById), new { response.Id }, response);
    }

    [HttpPut]
    public async Task<ActionResult<UpdatedBookingResponse>> Update([FromBody] UpdateBookingCommand command)
    {
        UpdatedBookingResponse response = await Mediator.Send(command);

        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<DeletedBookingResponse>> Delete([FromRoute] Guid id)
    {
        DeleteBookingCommand command = new() { Id = id };

        DeletedBookingResponse response = await Mediator.Send(command);

        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GetByIdBookingResponse>> GetById([FromRoute] Guid id)
    {
        GetByIdBookingQuery query = new() { Id = id };

        GetByIdBookingResponse response = await Mediator.Send(query);

        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<GetListResponse<GetListBookingListItemDto>>> GetList([FromQuery] PageRequest pageRequest)
    {
        GetListBookingQuery query = new() { PageRequest = pageRequest };

        GetListResponse<GetListBookingListItemDto> response = await Mediator.Send(query);

        return Ok(response);
    }

    [HttpPost("GetListByDynamic")]
    public async Task<IActionResult> GetListByDynamic([FromQuery] PageRequest pageRequest, [FromBody] DynamicQuery dynamic)
    {
        GetListByDynamicBookingQuery getListByDynamicBookingQuery = new() { PageRequest = pageRequest, Dynamic = dynamic };
        GetListResponse<GetListByDynamicBookingListItemDto> response = await Mediator.Send(getListByDynamicBookingQuery);
        return Ok(response);
    }
}