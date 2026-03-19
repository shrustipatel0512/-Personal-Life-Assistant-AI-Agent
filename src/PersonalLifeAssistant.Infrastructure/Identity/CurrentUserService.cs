using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using PersonalLifeAssistant.Application.Common.Interfaces;

namespace PersonalLifeAssistant.Infrastructure.Identity;

public class CurrentUserService : ICurrentUserService
{
    public static readonly Guid DemoUserId = Guid.Parse("11111111-1111-1111-1111-111111111111");
    public const string DemoEmail = "demo@assistant.local";
    public const string DemoTimeZone = "Asia/Kolkata";

    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid UserId
    {
        get
        {
            var claim = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.TryParse(claim, out var userId) ? userId : DemoUserId;
        }
    }

    public string Email => _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Email) ?? DemoEmail;

    public string TimeZone => _httpContextAccessor.HttpContext?.User.FindFirst("timezone")?.Value ?? DemoTimeZone;
}
