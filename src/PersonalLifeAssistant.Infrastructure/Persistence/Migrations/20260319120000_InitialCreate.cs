using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using PersonalLifeAssistant.Infrastructure.Persistence;

namespace PersonalLifeAssistant.Infrastructure.Persistence.Migrations;

[DbContext(typeof(ApplicationDbContext))]
[Migration("20260319120000_InitialCreate")]
public partial class InitialCreate : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "users",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                CreatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                UpdatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                FullName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                Email = table.Column<string>(type: "character varying(320)", maxLength: 320, nullable: false),
                PasswordHash = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false),
                TimeZone = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                Role = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                PreferencesJson = table.Column<string>(type: "text", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_users", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "chat_history",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                CreatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                UpdatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                UserId = table.Column<Guid>(type: "uuid", nullable: false),
                Role = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                Message = table.Column<string>(type: "character varying(8000)", maxLength: 8000, nullable: false),
                Intent = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                ContextSnapshotJson = table.Column<string>(type: "text", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_chat_history", x => x.Id);
                table.ForeignKey(
                    name: "FK_chat_history_users_UserId",
                    column: x => x.UserId,
                    principalTable: "users",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "expenses",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                CreatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                UpdatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                UserId = table.Column<Guid>(type: "uuid", nullable: false),
                Amount = table.Column<decimal>(type: "numeric", nullable: false),
                Currency = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                Category = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                OccurredOnUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                Notes = table.Column<string>(type: "text", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_expenses", x => x.Id);
                table.ForeignKey(
                    name: "FK_expenses_users_UserId",
                    column: x => x.UserId,
                    principalTable: "users",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "habits",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                CreatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                UpdatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                UserId = table.Column<Guid>(type: "uuid", nullable: false),
                Name = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                Frequency = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                TargetValue = table.Column<int>(type: "integer", nullable: false),
                CurrentValue = table.Column<int>(type: "integer", nullable: false),
                Unit = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                AiSuggestion = table.Column<string>(type: "text", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_habits", x => x.Id);
                table.ForeignKey(
                    name: "FK_habits_users_UserId",
                    column: x => x.UserId,
                    principalTable: "users",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "notifications",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                CreatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                UpdatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                UserId = table.Column<Guid>(type: "uuid", nullable: false),
                Channel = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                Message = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                ScheduledForUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                IsSent = table.Column<bool>(type: "boolean", nullable: false),
                MetadataJson = table.Column<string>(type: "text", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_notifications", x => x.Id);
                table.ForeignKey(
                    name: "FK_notifications_users_UserId",
                    column: x => x.UserId,
                    principalTable: "users",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "tasks",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                CreatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                UpdatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                UserId = table.Column<Guid>(type: "uuid", nullable: false),
                Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                Description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                Category = table.Column<int>(type: "integer", nullable: false),
                Priority = table.Column<int>(type: "integer", nullable: false),
                IsCompleted = table.Column<bool>(type: "boolean", nullable: false),
                DueDateUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                EstimatedMinutes = table.Column<int>(type: "integer", nullable: false),
                AiReasoning = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_tasks", x => x.Id);
                table.ForeignKey(
                    name: "FK_tasks_users_UserId",
                    column: x => x.UserId,
                    principalTable: "users",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_chat_history_UserId",
            table: "chat_history",
            column: "UserId");

        migrationBuilder.CreateIndex(
            name: "IX_expenses_UserId",
            table: "expenses",
            column: "UserId");

        migrationBuilder.CreateIndex(
            name: "IX_habits_UserId",
            table: "habits",
            column: "UserId");

        migrationBuilder.CreateIndex(
            name: "IX_notifications_UserId",
            table: "notifications",
            column: "UserId");

        migrationBuilder.CreateIndex(
            name: "IX_tasks_UserId",
            table: "tasks",
            column: "UserId");

        migrationBuilder.CreateIndex(
            name: "IX_users_Email",
            table: "users",
            column: "Email",
            unique: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "chat_history");
        migrationBuilder.DropTable(name: "expenses");
        migrationBuilder.DropTable(name: "habits");
        migrationBuilder.DropTable(name: "notifications");
        migrationBuilder.DropTable(name: "tasks");
        migrationBuilder.DropTable(name: "users");
    }

    protected override void BuildTargetModel(ModelBuilder modelBuilder)
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
