using PersonalLifeAssistant.Domain.Entities;

namespace PersonalLifeAssistant.Application.Common.Interfaces;

public interface ITaskRepository : IRepository<TaskItem>
{
    Task<IReadOnlyList<TaskItem>> GetByUserAsync(Guid userId, CancellationToken cancellationToken = default);
}
