using MediatR;
using Microsoft.AspNetCore.Mvc;
using PersonalLifeAssistant.Application.Features.Chat.Commands.SendChatMessage;

namespace PersonalLifeAssistant.Api.Controllers;

[ApiController]
[Route("api/chat")]
public class ChatController : ControllerBase
{
    private readonly ISender _sender;

    public ChatController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    public async Task<IActionResult> Chat([FromBody] ChatRequest request, CancellationToken cancellationToken)
        => Ok(await _sender.Send(new SendChatMessageCommand(request.Message), cancellationToken));
}

public class ChatRequest
{
    public string Message { get; set; } = string.Empty;
}
