using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PersonalLifeAssistant.Infrastructure.Persistence;

namespace PersonalLifeAssistant.Infrastructure.Persistence.Migrations;

[DbContext(typeof(ApplicationDbContext))]
partial class ApplicationDbContextModelSnapshot : ModelSnapshot
{
    protected override void BuildModel(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasAnnotation("ProductVersion", "8.0.4")
            .HasAnnotation("Relational:MaxIdentifierLength", 63);

        modelBuilder.Entity("PersonalLifeAssistant.Domain.Entities.AppUser", b =>
        {
            b.Property<Guid>("Id")
                .HasColumnType("uuid");

            b.Property<DateTimeOffset>("CreatedAtUtc")
                .HasColumnType("timestamp with time zone");

            b.Property<string>("Email")
                .IsRequired()
                .HasMaxLength(320)
                .HasColumnType("character varying(320)");

            b.Property<string>("FullName")
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnType("character varying(200)");

            b.Property<string>("PasswordHash")
                .IsRequired()
                .HasMaxLength(512)
                .HasColumnType("character varying(512)");

            b.Property<string>("PreferencesJson")
                .IsRequired()
                .HasColumnType("text");

            b.Property<string>("Role")
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnType("character varying(50)");

            b.Property<string>("TimeZone")
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnType("character varying(100)");

            b.Property<DateTimeOffset?>("UpdatedAtUtc")
                .HasColumnType("timestamp with time zone");

            b.HasKey("Id");

            b.HasIndex("Email")
                .IsUnique();

            b.ToTable("users", (string?)null);
        });

        modelBuilder.Entity("PersonalLifeAssistant.Domain.Entities.ChatHistory", b =>
        {
            b.Property<Guid>("Id")
                .HasColumnType("uuid");

            b.Property<string>("ContextSnapshotJson")
                .IsRequired()
                .HasColumnType("text");

            b.Property<DateTimeOffset>("CreatedAtUtc")
                .HasColumnType("timestamp with time zone");

            b.Property<string>("Intent")
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnType("character varying(100)");

            b.Property<string>("Message")
                .IsRequired()
                .HasMaxLength(8000)
                .HasColumnType("character varying(8000)");

            b.Property<string>("Role")
                .IsRequired()
                .HasMaxLength(30)
                .HasColumnType("character varying(30)");

            b.Property<DateTimeOffset?>("UpdatedAtUtc")
                .HasColumnType("timestamp with time zone");

            b.Property<Guid>("UserId")
                .HasColumnType("uuid");

            b.HasKey("Id");

            b.HasIndex("UserId");

            b.ToTable("chat_history", (string?)null);
        });

        modelBuilder.Entity("PersonalLifeAssistant.Domain.Entities.Expense", b =>
        {
            b.Property<Guid>("Id")
                .HasColumnType("uuid");

            b.Property<decimal>("Amount")
                .HasColumnType("numeric");

            b.Property<string>("Category")
                .IsRequired()
                .HasMaxLength(120)
                .HasColumnType("character varying(120)");

            b.Property<DateTimeOffset>("CreatedAtUtc")
                .HasColumnType("timestamp with time zone");

            b.Property<string>("Currency")
                .IsRequired()
                .HasMaxLength(10)
                .HasColumnType("character varying(10)");

            b.Property<string>("Notes")
                .HasColumnType("text");

            b.Property<DateTimeOffset>("OccurredOnUtc")
                .HasColumnType("timestamp with time zone");

            b.Property<DateTimeOffset?>("UpdatedAtUtc")
                .HasColumnType("timestamp with time zone");

            b.Property<Guid>("UserId")
                .HasColumnType("uuid");

            b.HasKey("Id");

            b.HasIndex("UserId");

            b.ToTable("expenses", (string?)null);
        });

        modelBuilder.Entity("PersonalLifeAssistant.Domain.Entities.Habit", b =>
        {
            b.Property<Guid>("Id")
                .HasColumnType("uuid");

            b.Property<string>("AiSuggestion")
                .IsRequired()
                .HasColumnType("text");

            b.Property<DateTimeOffset>("CreatedAtUtc")
                .HasColumnType("timestamp with time zone");

            b.Property<int>("CurrentValue")
                .HasColumnType("integer");

            b.Property<string>("Frequency")
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnType("character varying(50)");

            b.Property<string>("Name")
                .IsRequired()
                .HasMaxLength(120)
                .HasColumnType("character varying(120)");

            b.Property<int>("TargetValue")
                .HasColumnType("integer");

            b.Property<string>("Unit")
                .IsRequired()
                .HasMaxLength(30)
                .HasColumnType("character varying(30)");

            b.Property<DateTimeOffset?>("UpdatedAtUtc")
                .HasColumnType("timestamp with time zone");

            b.Property<Guid>("UserId")
                .HasColumnType("uuid");

            b.HasKey("Id");

            b.HasIndex("UserId");

            b.ToTable("habits", (string?)null);
        });

        modelBuilder.Entity("PersonalLifeAssistant.Domain.Entities.Notification", b =>
        {
            b.Property<Guid>("Id")
                .HasColumnType("uuid");

            b.Property<string>("Channel")
                .IsRequired()
                .HasMaxLength(30)
                .HasColumnType("character varying(30)");

            b.Property<DateTimeOffset>("CreatedAtUtc")
                .HasColumnType("timestamp with time zone");

            b.Property<bool>("IsSent")
                .HasColumnType("boolean");

            b.Property<string>("Message")
                .IsRequired()
                .HasMaxLength(1000)
                .HasColumnType("character varying(1000)");

            b.Property<string>("MetadataJson")
                .IsRequired()
                .HasColumnType("text");

            b.Property<DateTimeOffset>("ScheduledForUtc")
                .HasColumnType("timestamp with time zone");

            b.Property<DateTimeOffset?>("UpdatedAtUtc")
                .HasColumnType("timestamp with time zone");

            b.Property<Guid>("UserId")
                .HasColumnType("uuid");

            b.HasKey("Id");

            b.HasIndex("UserId");

            b.ToTable("notifications", (string?)null);
        });

        modelBuilder.Entity("PersonalLifeAssistant.Domain.Entities.TaskItem", b =>
        {
            b.Property<Guid>("Id")
                .HasColumnType("uuid");

            b.Property<string>("AiReasoning")
                .IsRequired()
                .HasMaxLength(4000)
                .HasColumnType("character varying(4000)");

            b.Property<int>("Category")
                .HasColumnType("integer");

            b.Property<DateTimeOffset>("CreatedAtUtc")
                .HasColumnType("timestamp with time zone");

            b.Property<string>("Description")
                .HasMaxLength(2000)
                .HasColumnType("character varying(2000)");

            b.Property<DateTimeOffset?>("DueDateUtc")
                .HasColumnType("timestamp with time zone");

            b.Property<int>("EstimatedMinutes")
                .HasColumnType("integer");

            b.Property<bool>("IsCompleted")
                .HasColumnType("boolean");

            b.Property<int>("Priority")
                .HasColumnType("integer");

            b.Property<string>("Title")
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnType("character varying(200)");

            b.Property<DateTimeOffset?>("UpdatedAtUtc")
                .HasColumnType("timestamp with time zone");

            b.Property<Guid>("UserId")
                .HasColumnType("uuid");

            b.HasKey("Id");

            b.HasIndex("UserId");

            b.ToTable("tasks", (string?)null);
        });

        modelBuilder.Entity("PersonalLifeAssistant.Domain.Entities.ChatHistory", b =>
        {
            b.HasOne("PersonalLifeAssistant.Domain.Entities.AppUser", "User")
                .WithMany("ChatHistory")
                .HasForeignKey("UserId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            b.Navigation("User");
        });

        modelBuilder.Entity("PersonalLifeAssistant.Domain.Entities.Expense", b =>
        {
            b.HasOne("PersonalLifeAssistant.Domain.Entities.AppUser", "User")
                .WithMany("Expenses")
                .HasForeignKey("UserId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            b.Navigation("User");
        });

        modelBuilder.Entity("PersonalLifeAssistant.Domain.Entities.Habit", b =>
        {
            b.HasOne("PersonalLifeAssistant.Domain.Entities.AppUser", "User")
                .WithMany("Habits")
                .HasForeignKey("UserId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            b.Navigation("User");
        });

        modelBuilder.Entity("PersonalLifeAssistant.Domain.Entities.Notification", b =>
        {
            b.HasOne("PersonalLifeAssistant.Domain.Entities.AppUser", "User")
                .WithMany("Notifications")
                .HasForeignKey("UserId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            b.Navigation("User");
        });

        modelBuilder.Entity("PersonalLifeAssistant.Domain.Entities.TaskItem", b =>
        {
            b.HasOne("PersonalLifeAssistant.Domain.Entities.AppUser", "User")
                .WithMany("Tasks")
                .HasForeignKey("UserId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            b.Navigation("User");
        });

        modelBuilder.Entity("PersonalLifeAssistant.Domain.Entities.AppUser", b =>
        {
            b.Navigation("ChatHistory");
            b.Navigation("Expenses");
            b.Navigation("Habits");
            b.Navigation("Notifications");
            b.Navigation("Tasks");
        });
    }
}
