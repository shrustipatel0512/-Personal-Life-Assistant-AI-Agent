using PersonalLifeAssistant.Domain.Enums;

namespace PersonalLifeAssistant.Application.Common.Models;

public class AiGeneratedTaskAnalysis
{
    public TaskPriority SuggestedPriority { get; set; } = TaskPriority.Medium;
    public TaskCategory SuggestedCategory { get; set; } = TaskCategory.Personal;
    public string Reasoning { get; set; } = string.Empty;
}
