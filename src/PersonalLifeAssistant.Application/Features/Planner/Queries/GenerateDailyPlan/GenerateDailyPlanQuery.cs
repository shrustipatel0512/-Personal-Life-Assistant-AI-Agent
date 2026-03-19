using MediatR;
using PersonalLifeAssistant.Application.Common.Models;

namespace PersonalLifeAssistant.Application.Features.Planner.Queries.GenerateDailyPlan;

public record GenerateDailyPlanQuery() : IRequest<Result<IReadOnlyList<PlannerItemDto>>>;
