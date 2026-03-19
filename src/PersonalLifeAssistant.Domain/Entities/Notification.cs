using PersonalLifeAssistant.Domain.Common;

namespace PersonalLifeAssistant.Domain.Entities;

public class Notification : AuditableEntity
{
    public Guid UserId { get; set; }
    public AppUser? User { get; set; }
    public string Channel { get; set; } = "Push";
    public string Message { get; set; } = string.Empty;
    public DateTimeOffset ScheduledForUtc { get; set; }
    public bool IsSent { get; set; }
    public string MetadataJson { get; set; } = "{}";
}
