using MediatR;
using PersonalLifeAssistant.Application.Common.Interfaces;
using PersonalLifeAssistant.Application.Common.Models;
using PersonalLifeAssistant.Application.Features.Tasks.Models;
using PersonalLifeAssistant.Domain.Enums;

namespace PersonalLifeAssistant.Application.Features.Tasks.Commands.UpdateTask;

public class UpdateTaskCommandHandler : IRequestHandler<UpdateTaskCommand, Result<TaskSummaryDto>>
{
    private readonly ITaskRepository _taskRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;

    public UpdateTaskCommandHandler(
        ITaskRepository taskRepository,
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService)
    {
        _taskRepository = taskRepository;
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }

    public async Task<Result<TaskSummaryDto>> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
    {
        var task = await _taskRepository.GetByIdAsync(request.TaskId, cancellationToken);
        if (task is null || task.UserId != _currentUserService.UserId)
        {
            return Result<TaskSummaryDto>.Failure("Task not found.");
        }

        if (string.IsNullOrWhiteSpace(request.Request.Title))
        {
            return Result<TaskSummaryDto>.Failure("Task title is required.");
        }

        if (request.Request.EstimatedMinutes < 1)
        {
            return Result<TaskSummaryDto>.Failure("Estimated minutes must be greater than zero.");
        }

        if (!Enum.TryParse<TaskCategory>(request.Request.Category, true, out var category))
        {
            return Result<TaskSummaryDto>.Failure("Category is invalid.");
        }

        task.Title = request.Request.Title.Trim();
        task.Description = string.IsNullOrWhiteSpace(request.Request.Description)
            ? null
            : request.Request.Description.Trim();
        task.Category = category;
        task.DueDateUtc = request.Request.DueDateUtc;
        task.EstimatedMinutes = request.Request.EstimatedMinutes;
        task.IsCompleted = request.Request.IsCompleted;
        task.UpdatedAtUtc = DateTimeOffset.UtcNow;

        _taskRepository.Update(task);
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
        }, "Task updated successfully.");
    }
}
