using MediatR;
using Microsoft.AspNetCore.Mvc;
using Service.Cqrs.Commands.Assets.Requests;
using Service.Cqrs.Queries.Assets.Requests;

namespace Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AssetController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpPost]
    public async Task<IActionResult> AddAsset([FromBody] AddAssetCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetAssetById), new { id = result.Id }, result);
    }

    [HttpGet]
    public async Task<IActionResult> ListAssets()
    {
        var result = await _mediator.Send(new GetAssetsQuery());
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAssetById(string id)
    {
        var result = await _mediator.Send(new GetAssetByIdQuery(id));
        if (result is null)
            return NotFound();

        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsset(string id)
    {
        var result = await _mediator.Send(new DeleteAssetCommand(id));
        if (!result)
            return NotFound();

        return NoContent();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsset(string id, [FromBody] UpdateAssetCommand command)
    {
        if (id != command.Id)
            return BadRequest();

        var result = await _mediator.Send(command);

        return Ok(result);
    }
}