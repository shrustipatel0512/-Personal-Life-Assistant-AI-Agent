namespace PersonalLifeAssistant.Application.Common.Interfaces;

public interface ICurrentUserService
{
    Guid UserId { get; }
    string Email { get; }
    string TimeZone { get; }
}
