using MediatR;
using PersonalLifeAssistant.Application.Common.Interfaces;
using PersonalLifeAssistant.Application.Common.Models;
using PersonalLifeAssistant.Application.Features.Tasks.Models;

namespace PersonalLifeAssistant.Application.Features.Planner.Queries.GenerateDailyPlan;

public class GenerateDailyPlanQueryHandler : IRequestHandler<GenerateDailyPlanQuery, Result<IReadOnlyList<PlannerItemDto>>>
{
    private readonly ITaskRepository _taskRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IAiService _aiService;

    public GenerateDailyPlanQueryHandler(
        ITaskRepository taskRepository,
        ICurrentUserService currentUserService,
        IAiService aiService)
    {
        _taskRepository = taskRepository;
        _currentUserService = currentUserService;
        _aiService = aiService;
    }

    public async Task<Result<IReadOnlyList<PlannerItemDto>>> Handle(GenerateDailyPlanQuery request, CancellationToken cancellationToken)
    {
        var tasks = await _taskRepository.GetByUserAsync(_currentUserService.UserId, cancellationToken);
        var pendingTasks = tasks
            .Where(t => !t.IsCompleted)
            .OrderBy(t => t.DueDateUtc ?? DateTimeOffset.MaxValue)
            .ThenByDescending(t => t.Priority)
            .Select(t => new TaskSummaryDto
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                Category = t.Category.ToString(),
                Priority = t.Priority.ToString(),
                IsCompleted = t.IsCompleted,
                DueDateUtc = t.DueDateUtc,
                EstimatedMinutes = t.EstimatedMinutes
            })
            .Take(4)
            .ToList()
            .AsReadOnly();

        if (pendingTasks.Count == 0)
        {
            return Result<IReadOnlyList<PlannerItemDto>>.Success([], "No open tasks available for planning.");
        }

        var plan = await _aiService.GenerateDailyPlanAsync(
            $"timezone={_currentUserService.TimeZone};date={DateTime.UtcNow:yyyy-MM-dd}",
            pendingTasks,
            cancellationToken);

        return Result<IReadOnlyList<PlannerItemDto>>.Success(plan, $"Generated a plan from {pendingTasks.Count} task(s).");
    }
}
