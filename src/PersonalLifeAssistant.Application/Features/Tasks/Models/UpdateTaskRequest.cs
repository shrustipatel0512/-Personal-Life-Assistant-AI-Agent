namespace PersonalLifeAssistant.Application.Features.Tasks.Models;

public class UpdateTaskRequest
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Category { get; set; } = string.Empty;
    public DateTimeOffset? DueDateUtc { get; set; }
    public int EstimatedMinutes { get; set; }
    public bool IsCompleted { get; set; }
}
