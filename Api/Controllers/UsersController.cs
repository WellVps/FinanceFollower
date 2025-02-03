using MediatR;
using Microsoft.AspNetCore.Mvc;
using Service.Cqrs.Commands.Users.Requests;

namespace Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class UsersController(IMediator mediator): ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpPost]
    public async Task<IActionResult> SaveUser(
        [FromBody] CreateUserRequest request
    )
    {
        var response = await _mediator.Send(request);

        return Ok(response);
    }
}