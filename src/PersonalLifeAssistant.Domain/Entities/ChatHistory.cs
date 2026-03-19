using PersonalLifeAssistant.Domain.Common;

namespace PersonalLifeAssistant.Domain.Entities;

public class ChatHistory : AuditableEntity
{
    public Guid UserId { get; set; }
    public AppUser? User { get; set; }
    public string Role { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string Intent { get; set; } = string.Empty;
    public string ContextSnapshotJson { get; set; } = "{}";
}
