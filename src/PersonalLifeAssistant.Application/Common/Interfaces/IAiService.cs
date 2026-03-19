using PersonalLifeAssistant.Application.Common.Models;
using PersonalLifeAssistant.Application.Features.Chat.Models;
using PersonalLifeAssistant.Application.Features.Tasks.Models;

namespace PersonalLifeAssistant.Application.Common.Interfaces;

public interface IAiService
{
    Task<AiGeneratedTaskAnalysis> AnalyzeTaskAsync(CreateTaskRequest request, string userContext, CancellationToken cancellationToken);
    Task<IReadOnlyList<PlannerItemDto>> GenerateDailyPlanAsync(string userContext, IReadOnlyList<TaskSummaryDto> tasks, CancellationToken cancellationToken);
    Task<ChatReplyDto> SendChatAsync(string userMessage, string userContext, IReadOnlyList<string> recentMessages, CancellationToken cancellationToken);
}
