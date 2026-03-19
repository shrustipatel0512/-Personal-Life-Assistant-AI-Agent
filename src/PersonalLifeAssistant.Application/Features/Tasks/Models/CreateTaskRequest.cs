namespace PersonalLifeAssistant.Application.Features.Tasks.Models;

public class CreateTaskRequest
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTimeOffset? DueDateUtc { get; set; }
    public int EstimatedMinutes { get; set; }
}
