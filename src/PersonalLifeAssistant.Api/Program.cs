using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using PersonalLifeAssistant.Domain.Entities;
using PersonalLifeAssistant.Infrastructure.Persistence;
using PersonalLifeAssistant.Infrastructure.Identity;
using PersonalLifeAssistant.Application;
using PersonalLifeAssistant.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddProblemDetails();
builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("api", limiterOptions =>
    {
        limiterOptions.PermitLimit = 100;
        limiterOptions.Window = TimeSpan.FromMinutes(1);
        limiterOptions.QueueLimit = 0;
    });
});

var app = builder.Build();

await using (var scope = app.Services.CreateAsyncScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await dbContext.Database.MigrateAsync();

    var demoUserExists = await dbContext.Users.AnyAsync(
        user => user.Id == CurrentUserService.DemoUserId);

    if (!demoUserExists)
    {
        dbContext.Users.Add(new AppUser
        {
            Id = CurrentUserService.DemoUserId,
            FullName = "Demo User",
            Email = CurrentUserService.DemoEmail,
            PasswordHash = "demo-user-not-for-login",
            TimeZone = CurrentUserService.DemoTimeZone,
            Role = "User",
            PreferencesJson = "{}"
        });

        await dbContext.SaveChangesAsync();
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler();
app.UseRateLimiter();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers().RequireRateLimiting("api");

app.Run();
