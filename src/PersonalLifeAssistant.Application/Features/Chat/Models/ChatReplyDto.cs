namespace PersonalLifeAssistant.Application.Features.Chat.Models;

public class ChatReplyDto
{
    public string Response { get; set; } = string.Empty;
    public string Intent { get; set; } = string.Empty;
    public IReadOnlyList<string> SuggestedActions { get; set; } = Array.Empty<string>();
}
