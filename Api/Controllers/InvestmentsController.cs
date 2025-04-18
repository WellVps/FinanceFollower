using MediatR;
using Microsoft.AspNetCore.Mvc;
using Service.Cqrs.Commands.InvestmentsCqrs.Requests;
using Service.DTOs.InvestmentsDTOs;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InvestmentsController : ControllerBase
{
    [HttpPost()]
    public async Task<IActionResult> AddInvestmentEntry(
        [FromServices] IMediator mediator,
        [FromBody] List<InvestmentsEntriesDTO> investmentsEntries
    )
    {
        if (investmentsEntries == null || investmentsEntries.Count == 0)
        {
            return BadRequest("Investment entries are required");
        }

        var result = await mediator.Send(new AddInvestmentEntryCommand(investmentsEntries));

        if (result)
        {
            return Ok();
        }

        return BadRequest("Failed to add investment entries");
    }
}