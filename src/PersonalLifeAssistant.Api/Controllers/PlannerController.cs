using MediatR;
using Microsoft.AspNetCore.Mvc;
using PersonalLifeAssistant.Application.Features.Planner.Queries.GenerateDailyPlan;

namespace PersonalLifeAssistant.Api.Controllers;

[ApiController]
[Route("api/planner")]
public class PlannerController : ControllerBase
{
    private readonly ISender _sender;

    public PlannerController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost("generate")]
    public async Task<IActionResult> Generate(CancellationToken cancellationToken)
        => Ok(await _sender.Send(new GenerateDailyPlanQuery(), cancellationToken));
}
