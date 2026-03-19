using Microsoft.AspNetCore.Mvc;

namespace PersonalLifeAssistant.Api.Controllers;

[ApiController]
[Route("api/health")]
public class HealthController : ControllerBase
{
    [HttpGet]
    public IActionResult Get() => Ok(new
    {
        Status = "Healthy",
        Service = "Personal Life Assistant API",
        UtcNow = DateTimeOffset.UtcNow
    });
}
