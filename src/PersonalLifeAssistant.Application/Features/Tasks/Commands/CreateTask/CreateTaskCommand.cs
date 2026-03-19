using MediatR;
using PersonalLifeAssistant.Application.Common.Models;
using PersonalLifeAssistant.Application.Features.Tasks.Models;

namespace PersonalLifeAssistant.Application.Features.Tasks.Commands.CreateTask;

public record CreateTaskCommand(CreateTaskRequest Request) : IRequest<Result<TaskSummaryDto>>;
