using MediatR;
using PersonalLifeAssistant.Application.Common.Models;
using PersonalLifeAssistant.Application.Features.Tasks.Models;

namespace PersonalLifeAssistant.Application.Features.Tasks.Commands.UpdateTask;

public record UpdateTaskCommand(Guid TaskId, UpdateTaskRequest Request) : IRequest<Result<TaskSummaryDto>>;
