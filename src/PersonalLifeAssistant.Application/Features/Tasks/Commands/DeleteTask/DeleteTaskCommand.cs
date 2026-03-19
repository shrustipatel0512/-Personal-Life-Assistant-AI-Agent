using MediatR;
using PersonalLifeAssistant.Application.Common.Models;

namespace PersonalLifeAssistant.Application.Features.Tasks.Commands.DeleteTask;

public record DeleteTaskCommand(Guid TaskId) : IRequest<Result<bool>>;
