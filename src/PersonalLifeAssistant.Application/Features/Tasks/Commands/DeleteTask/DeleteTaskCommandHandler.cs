using MediatR;
using PersonalLifeAssistant.Application.Common.Interfaces;
using PersonalLifeAssistant.Application.Common.Models;

namespace PersonalLifeAssistant.Application.Features.Tasks.Commands.DeleteTask;

public class DeleteTaskCommandHandler : IRequestHandler<DeleteTaskCommand, Result<bool>>
{
    private readonly ITaskRepository _taskRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;

    public DeleteTaskCommandHandler(
        ITaskRepository taskRepository,
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService)
    {
        _taskRepository = taskRepository;
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }

    public async Task<Result<bool>> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
    {
        var task = await _taskRepository.GetByIdAsync(request.TaskId, cancellationToken);
        if (task is null || task.UserId != _currentUserService.UserId)
        {
            return Result<bool>.Failure("Task not found.");
        }

        _taskRepository.Remove(task);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<bool>.Success(true, "Task deleted successfully.");
    }
}
