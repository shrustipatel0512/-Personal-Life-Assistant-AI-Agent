using PersonalLifeAssistant.Domain.Common;

namespace PersonalLifeAssistant.Domain.Entities;

public class AppUser : AuditableEntity
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string TimeZone { get; set; } = "UTC";
    public string Role { get; set; } = "User";
    public string PreferencesJson { get; set; } = "{}";

    public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
    public ICollection<Habit> Habits { get; set; } = new List<Habit>();
    public ICollection<Expense> Expenses { get; set; } = new List<Expense>();
    public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    public ICollection<ChatHistory> ChatHistory { get; set; } = new List<ChatHistory>();
}
