using PersonalLifeAssistant.Domain.Common;

namespace PersonalLifeAssistant.Domain.Entities;

public class Expense : AuditableEntity
{
    public Guid UserId { get; set; }
    public AppUser? User { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "USD";
    public string Category { get; set; } = string.Empty;
    public DateTimeOffset OccurredOnUtc { get; set; }
    public string? Notes { get; set; }
}
