using MediatR;
using Microsoft.AspNetCore.Mvc;
using Service.Cqrs.Commands.Assets.Requests;
using Service.Cqrs.Queries.Assets.Requests;

namespace Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AssetTypeController(IMediator mediator): ControllerBase
{
    private readonly IMediator _mediator = mediator;

    /// <summary>
    /// Create a new Asset Type
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> SaveAssetType(
        [FromBody] CreateAssetTypeCommand request
    )
    {
        var response = await _mediator.Send(request);

        return Ok(response);
    }

    [HttpGet]
    public async Task<IActionResult> GetAssetTypes()
    {
        var response = await _mediator.Send(new GetAssetTypesQuery());

        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAssetTypeById(string id)
    {
        var response = await _mediator.Send(new GetAssetTypeByIdQuery(id));

        if(response is null)
            return NotFound();

        return Ok(response);
    }   

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAssetType(string id, [FromBody] UpdateAssetTypeCommand request)
    {
        request.Id = id;
        var response = await _mediator.Send(request);

        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAssetType(string id)
    {
        var response = await _mediator.Send(new DeleteAssetTypeCommand(id));

        return Ok(response);
    }
}