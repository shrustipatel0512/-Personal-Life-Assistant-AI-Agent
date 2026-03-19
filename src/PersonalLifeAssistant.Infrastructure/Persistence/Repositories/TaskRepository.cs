using Microsoft.EntityFrameworkCore;
using PersonalLifeAssistant.Application.Common.Interfaces;
using PersonalLifeAssistant.Domain.Entities;

namespace PersonalLifeAssistant.Infrastructure.Persistence.Repositories;

public class TaskRepository : EfRepository<TaskItem>, ITaskRepository
{
    public TaskRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<IReadOnlyList<TaskItem>> GetByUserAsync(Guid userId, CancellationToken cancellationToken = default)
        => await DbContext.Tasks
            .Where(x => x.UserId == userId)
            .OrderBy(x => x.DueDateUtc)
            .ToListAsync(cancellationToken);
}
