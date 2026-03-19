namespace PersonalLifeAssistant.Application.Features.Tasks.Models;

public class TaskSummaryDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Category { get; set; } = string.Empty;
    public string Priority { get; set; } = string.Empty;
    public bool IsCompleted { get; set; }
    public DateTimeOffset? DueDateUtc { get; set; }
    public int EstimatedMinutes { get; set; }
}
