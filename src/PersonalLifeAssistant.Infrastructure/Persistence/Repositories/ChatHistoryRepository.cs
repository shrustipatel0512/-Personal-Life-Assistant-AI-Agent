using Microsoft.EntityFrameworkCore;
using PersonalLifeAssistant.Application.Common.Interfaces;
using PersonalLifeAssistant.Domain.Entities;

namespace PersonalLifeAssistant.Infrastructure.Persistence.Repositories;

public class ChatHistoryRepository : EfRepository<ChatHistory>, IChatHistoryRepository
{
    public ChatHistoryRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<IReadOnlyList<ChatHistory>> GetRecentByUserAsync(Guid userId, int limit, CancellationToken cancellationToken = default)
        => await DbContext.ChatHistory
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.CreatedAtUtc)
            .Take(limit)
            .OrderBy(x => x.CreatedAtUtc)
            .ToListAsync(cancellationToken);
}
