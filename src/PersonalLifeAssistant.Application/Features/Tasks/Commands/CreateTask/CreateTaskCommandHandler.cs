using MediatR;
using PersonalLifeAssistant.Application.Common.Interfaces;
using PersonalLifeAssistant.Application.Common.Models;
using PersonalLifeAssistant.Application.Features.Tasks.Models;
using PersonalLifeAssistant.Domain.Entities;

namespace PersonalLifeAssistant.Application.Features.Tasks.Commands.CreateTask;

public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, Result<TaskSummaryDto>>
{
    private readonly ITaskRepository _taskRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAiService _aiService;
    private readonly ICurrentUserService _currentUserService;

    public CreateTaskCommandHandler(
        ITaskRepository taskRepository,
        IUnitOfWork unitOfWork,
        IAiService aiService,
        ICurrentUserService currentUserService)
    {
        _taskRepository = taskRepository;
        _unitOfWork = unitOfWork;
        _aiService = aiService;
        _currentUserService = currentUserService;
    }

    public async Task<Result<TaskSummaryDto>> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
    {
        var aiAnalysis = await _aiService.AnalyzeTaskAsync(
            request.Request,
            $"timezone={_currentUserService.TimeZone};email={_currentUserService.Email}",
            cancellationToken);

        var task = new TaskItem
        {
            UserId = _currentUserService.UserId,
            Title = request.Request.Title,
            Description = request.Request.Description,
            DueDateUtc = request.Request.DueDateUtc,
            EstimatedMinutes = request.Request.EstimatedMinutes,
            Priority = aiAnalysis.SuggestedPriority,
            Category = aiAnalysis.SuggestedCategory,
            AiReasoning = aiAnalysis.Reasoning
        };

        await _taskRepository.AddAsync(task, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<TaskSummaryDto>.Success(new TaskSummaryDto
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            Category = task.Category.ToString(),
            Priority = task.Priority.ToString(),
            IsCompleted = task.IsCompleted,
            DueDateUtc = task.DueDateUtc,
            EstimatedMinutes = task.EstimatedMinutes
        }, "Task created with Gemini-assisted prioritization.");
    }
}
