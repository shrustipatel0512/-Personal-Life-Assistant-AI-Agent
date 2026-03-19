using PersonalLifeAssistant.Domain.Common;

namespace PersonalLifeAssistant.Domain.Entities;

public class Habit : AuditableEntity
{
    public Guid UserId { get; set; }
    public AppUser? User { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Frequency { get; set; } = "Daily";
    public int TargetValue { get; set; }
    public int CurrentValue { get; set; }
    public string Unit { get; set; } = string.Empty;
    public string AiSuggestion { get; set; } = string.Empty;
}
