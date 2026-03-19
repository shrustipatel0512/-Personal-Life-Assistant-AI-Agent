using PersonalLifeAssistant.Domain.Common;
using PersonalLifeAssistant.Domain.Enums;

namespace PersonalLifeAssistant.Domain.Entities;

public class TaskItem : AuditableEntity
{
    public Guid UserId { get; set; }
    public AppUser? User { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public TaskCategory Category { get; set; }
    public TaskPriority Priority { get; set; } = TaskPriority.Medium;
    public bool IsCompleted { get; set; }
    public DateTimeOffset? DueDateUtc { get; set; }
    public int EstimatedMinutes { get; set; }
    public string AiReasoning { get; set; } = string.Empty;
}
