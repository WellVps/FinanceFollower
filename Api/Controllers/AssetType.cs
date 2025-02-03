using MediatR;
using Microsoft.AspNetCore.Mvc;
using Service.Cqrs.Commands.Assets.Requests;

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
        [FromBody] CreateAssetTypeRequest request
    )
    {
        var response = await _mediator.Send(request);

        return Ok(response);
    }

    [HttpGet]
    public async Task<IActionResult> GetAssetTypes()
    {
        var response = await _mediator.Send(new GetAssetTypesRequest());

        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAssetTypeById(int id)
    {
        var response = await _mediator.Send(new GetAssetTypeByIdRequest(id));

        return Ok(response);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAssetType(int id, [FromBody] UpdateAssetTypeRequest request)
    {
        request.Id = id;
        var response = await _mediator.Send(request);

        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAssetType(int id)
    {
        var response = await _mediator.Send(new DeleteAssetTypeRequest(id));

        return Ok(response);
    }
}