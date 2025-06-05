using MediatR;
using BaseApi.Extensions;
using Microsoft.AspNetCore.Mvc;
using Service.DTOs.Authentication;
using Service.Cqrs.Commands.Authentication.Requests;

namespace Api.Controllers;

[ApiController]
[Route("/api/v1/[controller]")]
[ApiExplorerSettings(IgnoreApi = false)]
public class AuthenticationController(
    IConfiguration configuration,
    IMediator mediator
) : ControllerBase
{
    [HttpPost]
    [Route("Authenticate")]
    public async Task<IActionResult> Authenticate(
        [FromBody] AuthenticationDto body,
        CancellationToken cancellationToken
    )
    {
        var command = new AuthenticationRequest
        {
            Email = body.Email,
            Password = body.Password,
            TokenConfigs = configuration.GetUserTokenConfiguration()
        };

        return Ok(await mediator.Send(command, cancellationToken));
    }

    [HttpPost]
    [Route("Refresh")]
    public async Task<IActionResult> RefreshToken(
        [FromBody] RefreshTokenDto body,
        CancellationToken cancellationToken
    )
    {
        var command = new RefreshTokenRequest()
        {
            RefreshToken = body.RefreshToken,
            TokenConfigs = configuration.GetUserTokenConfiguration()
        };

        return Ok(await mediator.Send(command, cancellationToken));
    }
}