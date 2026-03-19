using Microsoft.EntityFrameworkCore;
using PersonalLifeAssistant.Domain.Entities;

namespace PersonalLifeAssistant.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<AppUser> Users => Set<AppUser>();
    public DbSet<TaskItem> Tasks => Set<TaskItem>();
    public DbSet<Habit> Habits => Set<Habit>();
    public DbSet<Expense> Expenses => Set<Expense>();
    public DbSet<Notification> Notifications => Set<Notification>();
    public DbSet<ChatHistory> ChatHistory => Set<ChatHistory>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<AppUser>(entity =>
        {
            entity.ToTable("users");
            entity.HasIndex(x => x.Email).IsUnique();
            entity.Property(x => x.FullName).HasMaxLength(200).IsRequired();
            entity.Property(x => x.Email).HasMaxLength(320).IsRequired();
            entity.Property(x => x.PasswordHash).HasMaxLength(512).IsRequired();
            entity.Property(x => x.Role).HasMaxLength(50).IsRequired();
            entity.Property(x => x.TimeZone).HasMaxLength(100).IsRequired();
        });

        modelBuilder.Entity<TaskItem>(entity =>
        {
            entity.ToTable("tasks");
            entity.Property(x => x.Title).HasMaxLength(200).IsRequired();
            entity.Property(x => x.Description).HasMaxLength(2000);
            entity.Property(x => x.AiReasoning).HasMaxLength(4000);
            entity.HasOne(x => x.User).WithMany(x => x.Tasks).HasForeignKey(x => x.UserId);
        });

        modelBuilder.Entity<Habit>(entity =>
        {
            entity.ToTable("habits");
            entity.Property(x => x.Name).HasMaxLength(120).IsRequired();
            entity.Property(x => x.Frequency).HasMaxLength(50).IsRequired();
            entity.Property(x => x.Unit).HasMaxLength(30).IsRequired();
            entity.HasOne(x => x.User).WithMany(x => x.Habits).HasForeignKey(x => x.UserId);
        });

        modelBuilder.Entity<Expense>(entity =>
        {
            entity.ToTable("expenses");
            entity.Property(x => x.Category).HasMaxLength(120).IsRequired();
            entity.Property(x => x.Currency).HasMaxLength(10).IsRequired();
            entity.HasOne(x => x.User).WithMany(x => x.Expenses).HasForeignKey(x => x.UserId);
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.ToTable("notifications");
            entity.Property(x => x.Channel).HasMaxLength(30).IsRequired();
            entity.Property(x => x.Message).HasMaxLength(1000).IsRequired();
            entity.HasOne(x => x.User).WithMany(x => x.Notifications).HasForeignKey(x => x.UserId);
        });

        modelBuilder.Entity<ChatHistory>(entity =>
        {
            entity.ToTable("chat_history");
            entity.Property(x => x.Role).HasMaxLength(30).IsRequired();
            entity.Property(x => x.Intent).HasMaxLength(100).IsRequired();
            entity.Property(x => x.Message).HasMaxLength(8000).IsRequired();
            entity.HasOne(x => x.User).WithMany(x => x.ChatHistory).HasForeignKey(x => x.UserId);
        });
    }
}
