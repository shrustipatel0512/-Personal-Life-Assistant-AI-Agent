using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Options;
using PersonalLifeAssistant.Application.Common.Interfaces;
using PersonalLifeAssistant.Application.Common.Models;
using PersonalLifeAssistant.Application.Features.Chat.Models;
using PersonalLifeAssistant.Application.Features.Tasks.Models;
using PersonalLifeAssistant.Domain.Enums;

namespace PersonalLifeAssistant.Infrastructure.AI;

public class GeminiAiService : IAiService
{
    private readonly HttpClient _httpClient;
    private readonly GeminiOptions _options;

    public GeminiAiService(HttpClient httpClient, IOptions<GeminiOptions> options)
    {
        _httpClient = httpClient;
        _options = options.Value;
    }

    public async Task<AiGeneratedTaskAnalysis> AnalyzeTaskAsync(CreateTaskRequest request, string userContext, CancellationToken cancellationToken)
    {
        if (!HasConfiguredApiKey())
        {
            return new AiGeneratedTaskAnalysis
            {
                SuggestedPriority = request.DueDateUtc is not null && request.DueDateUtc <= DateTimeOffset.UtcNow.AddDays(1)
                    ? TaskPriority.High
                    : TaskPriority.Medium,
                SuggestedCategory = InferCategory(request.Title, request.Description),
                Reasoning = "Fallback prioritization was used because Gemini API is not configured yet."
            };
        }

        var prompt = $"""
        You are an AI task-prioritization engine for a personal life assistant app.
        Return JSON with keys: suggestedPriority, suggestedCategory, reasoning.
        User context: {userContext}
        Task title: {request.Title}
        Task description: {request.Description}
        Due date UTC: {request.DueDateUtc}
        Estimated minutes: {request.EstimatedMinutes}
        Valid priorities: Low, Medium, High, Critical.
        Valid categories: Work, Personal, Health, Finance, Family.
        """;

        var responseText = await SendPromptAsync(prompt, cancellationToken);
        return ParseTaskAnalysis(responseText);
    }

    public async Task<IReadOnlyList<PlannerItemDto>> GenerateDailyPlanAsync(string userContext, IReadOnlyList<TaskSummaryDto> tasks, CancellationToken cancellationToken)
    {
        if (tasks.Count == 0)
        {
            return [];
        }

        if (!HasConfiguredApiKey())
        {
            return BuildFallbackPlan(tasks);
        }

        var tasksJson = JsonSerializer.Serialize(tasks);
        var prompt = $"""
        You are a daily planner AI. Create a balanced daily plan.
        Return a JSON array. Each item must include: title, startUtc, endUtc, recommendation.
        User context: {userContext}
        Tasks: {tasksJson}
        Constraints:
        - Focus on tasks with the earliest due dates first.
        - Prefer high-priority items when due dates are similar.
        - Do not try to schedule every task in one day if there are many tasks.
        - Use at most 4 focused task blocks for the day.
        - Keep breaks between deep-work blocks.
        - Avoid overloading beyond 8 productive hours.
        - Only return a plan for the provided tasks.
        """;

        var responseText = await SendPromptAsync(prompt, cancellationToken);
        return JsonSerializer.Deserialize<List<PlannerItemDto>>(responseText, JsonOptions()) ?? [];
    }

    public async Task<ChatReplyDto> SendChatAsync(string userMessage, string userContext, IReadOnlyList<string> recentMessages, CancellationToken cancellationToken)
    {
        if (!HasConfiguredApiKey())
        {
            return new ChatReplyDto
            {
                Response = $"You said: \"{userMessage}\". Gemini is not configured yet, so I am replying with the local fallback assistant. I can still help you organize tasks and suggest your next focus area.",
                Intent = "fallback_chat",
                SuggestedActions = new[] { "Create a high-priority task", "Generate today's plan" }
            };
        }

        var history = string.Join(Environment.NewLine, recentMessages);
        var prompt = $"""
        You are a conversational personal life assistant.
        Respond helpfully and briefly. If possible, suggest actionable next steps.
        Return JSON with keys: response, intent, suggestedActions.
        User context: {userContext}
        Recent messages:
        {history}

        User message:
        {userMessage}
        """;

        var responseText = await SendPromptAsync(prompt, cancellationToken);
        return JsonSerializer.Deserialize<ChatReplyDto>(responseText, JsonOptions()) ?? new ChatReplyDto
        {
            Response = "I can help plan your day, prioritize tasks, and suggest reminders.",
            Intent = "assistant_response",
            SuggestedActions = new[] { "Open planner", "Review today's tasks" }
        };
    }

    private async Task<string> SendPromptAsync(string prompt, CancellationToken cancellationToken)
    {
        var endpoint = $"{_options.BaseUrl}/models/{_options.Model}:generateContent?key={_options.ApiKey}";
        var requestBody = new
        {
            contents = new[]
            {
                new
                {
                    parts = new[]
                    {
                        new { text = prompt }
                    }
                }
            },
            generationConfig = new
            {
                temperature = 0.3,
                responseMimeType = "application/json"
            }
        };

        using var response = await _httpClient.PostAsJsonAsync(endpoint, requestBody, cancellationToken);
        response.EnsureSuccessStatusCode();

        var geminiResponse = await response.Content.ReadFromJsonAsync<GeminiResponse>(cancellationToken: cancellationToken);
        return geminiResponse?.Candidates?.FirstOrDefault()?.Content?.Parts?.FirstOrDefault()?.Text ?? "{}";
    }

    private bool HasConfiguredApiKey()
        => !string.IsNullOrWhiteSpace(_options.ApiKey) &&
           !_options.ApiKey.Contains("YOUR_GEMINI_API_KEY", StringComparison.OrdinalIgnoreCase);

    private static TaskCategory InferCategory(string title, string? description)
    {
        var text = $"{title} {description}".ToLowerInvariant();

        if (text.Contains("gym") || text.Contains("water") || text.Contains("sleep") || text.Contains("health"))
        {
            return TaskCategory.Health;
        }

        if (text.Contains("money") || text.Contains("expense") || text.Contains("bill") || text.Contains("tax"))
        {
            return TaskCategory.Finance;
        }

        if (text.Contains("mom") || text.Contains("dad") || text.Contains("family"))
        {
            return TaskCategory.Family;
        }

        if (text.Contains("office") || text.Contains("meeting") || text.Contains("project") || text.Contains("client"))
        {
            return TaskCategory.Work;
        }

        return TaskCategory.Personal;
    }

    private static IReadOnlyList<PlannerItemDto> BuildFallbackPlan(IReadOnlyList<TaskSummaryDto> tasks)
    {
        if (tasks.Count == 0)
        {
            return [];
        }

        var start = DateTimeOffset.UtcNow.Date.AddHours(9);
        var plan = new List<PlannerItemDto>();

        foreach (var task in tasks)
        {
            var blockMinutes = Math.Clamp(task.EstimatedMinutes, 30, 120);
            var end = start.AddMinutes(blockMinutes);
            plan.Add(new PlannerItemDto
            {
                Title = task.Title,
                StartUtc = start,
                EndUtc = end,
                Recommendation = task.DueDateUtc is not null
                    ? $"Work on this {task.Priority.ToLowerInvariant()} priority task before its due date on {task.DueDateUtc:ddd, MMM d h:mm tt}."
                    : $"Work on this {task.Priority.ToLowerInvariant()} priority task in a focused block."
            });

            start = end.AddMinutes(15);
        }

        return plan;
    }

    private static AiGeneratedTaskAnalysis ParseTaskAnalysis(string json)
    {
        using var document = JsonDocument.Parse(json);
        var root = document.RootElement;

        var priorityText = root.GetProperty("suggestedPriority").GetString() ?? "Medium";
        var categoryText = root.GetProperty("suggestedCategory").GetString() ?? "Personal";
        var reasoning = root.GetProperty("reasoning").GetString() ?? string.Empty;

        Enum.TryParse(priorityText, true, out TaskPriority priority);
        Enum.TryParse(categoryText, true, out TaskCategory category);

        return new AiGeneratedTaskAnalysis
        {
            SuggestedPriority = priority == 0 ? TaskPriority.Medium : priority,
            SuggestedCategory = category == 0 ? TaskCategory.Personal : category,
            Reasoning = reasoning
        };
    }

    private static JsonSerializerOptions JsonOptions() => new()
    {
        PropertyNameCaseInsensitive = true
    };

    private sealed class GeminiResponse
    {
        public List<Candidate>? Candidates { get; set; }
    }

    private sealed class Candidate
    {
        public Content? Content { get; set; }
    }

    private sealed class Content
    {
        public List<Part>? Parts { get; set; }
    }

    private sealed class Part
    {
        public string? Text { get; set; }
    }
}
