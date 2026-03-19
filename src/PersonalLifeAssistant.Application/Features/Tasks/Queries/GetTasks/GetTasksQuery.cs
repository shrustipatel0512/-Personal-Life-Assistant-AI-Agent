using MediatR;
using PersonalLifeAssistant.Application.Common.Models;
using PersonalLifeAssistant.Application.Features.Tasks.Models;

namespace PersonalLifeAssistant.Application.Features.Tasks.Queries.GetTasks;

public record GetTasksQuery() : IRequest<Result<IReadOnlyList<TaskSummaryDto>>>;
