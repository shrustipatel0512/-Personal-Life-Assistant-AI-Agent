namespace PersonalLifeAssistant.Application.Common.Models;

public class PlannerItemDto
{
    public string Title { get; set; } = string.Empty;
    public DateTimeOffset StartUtc { get; set; }
    public DateTimeOffset EndUtc { get; set; }
    public string Recommendation { get; set; } = string.Empty;
}
