using Application.Features.Inventories.Commands.Create;
using Application.Features.Inventories.Commands.Delete;
using Application.Features.Inventories.Commands.Update;
using Application.Features.Inventories.Queries.GetById;
using Application.Features.Inventories.Queries.GetList;
using Application.Features.Inventories.Queries.GetListByDynamic;
using NArchitecture.Core.Persistence.Dynamic;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Application.Responses;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class InventoriesController : BaseController
{
    [HttpPost]
    public async Task<ActionResult<CreatedInventoryResponse>> Add([FromBody] CreateInventoryCommand command)
    {
        CreatedInventoryResponse response = await Mediator.Send(command);

        return CreatedAtAction(nameof(GetById), new { response.Id }, response);
    }

    [HttpPut]
    public async Task<ActionResult<UpdatedInventoryResponse>> Update([FromBody] UpdateInventoryCommand command)
    {
        UpdatedInventoryResponse response = await Mediator.Send(command);

        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<DeletedInventoryResponse>> Delete([FromRoute] Guid id)
    {
        DeleteInventoryCommand command = new() { Id = id };

        DeletedInventoryResponse response = await Mediator.Send(command);

        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GetByIdInventoryResponse>> GetById([FromRoute] Guid id)
    {
        GetByIdInventoryQuery query = new() { Id = id };

        GetByIdInventoryResponse response = await Mediator.Send(query);

        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<GetListResponse<GetListInventoryListItemDto>>> GetList([FromQuery] PageRequest pageRequest)
    {
        GetListInventoryQuery query = new() { PageRequest = pageRequest };

        GetListResponse<GetListInventoryListItemDto> response = await Mediator.Send(query);

        return Ok(response);
    }

    [HttpPost("GetListByDynamic")]
    public async Task<IActionResult> GetListByDynamic([FromQuery] PageRequest pageRequest, [FromBody] DynamicQuery dynamic)
    {
        GetListByDynamicInventoryQuery getListByDynamicInventoryQuery = new() { PageRequest = pageRequest, Dynamic = dynamic };
        GetListResponse<GetListByDynamicInventoryListItemDto> response = await Mediator.Send(getListByDynamicInventoryQuery);
        return Ok(response);
    }
}