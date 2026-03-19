using MediatR;
using PersonalLifeAssistant.Application.Common.Interfaces;
using PersonalLifeAssistant.Application.Common.Models;
using PersonalLifeAssistant.Application.Features.Tasks.Models;

namespace PersonalLifeAssistant.Application.Features.Tasks.Queries.GetTasks;

public class GetTasksQueryHandler : IRequestHandler<GetTasksQuery, Result<IReadOnlyList<TaskSummaryDto>>>
{
    private readonly ITaskRepository _taskRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetTasksQueryHandler(ITaskRepository taskRepository, ICurrentUserService currentUserService)
    {
        _taskRepository = taskRepository;
        _currentUserService = currentUserService;
    }

    public async Task<Result<IReadOnlyList<TaskSummaryDto>>> Handle(GetTasksQuery request, CancellationToken cancellationToken)
    {
        var tasks = await _taskRepository.GetByUserAsync(_currentUserService.UserId, cancellationToken);
        var result = tasks
            .OrderBy(t => t.IsCompleted)
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
            .ToList()
            .AsReadOnly();

        return Result<IReadOnlyList<TaskSummaryDto>>.Success(result);
    }
}
