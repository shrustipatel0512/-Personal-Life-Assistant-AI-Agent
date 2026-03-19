using PersonalLifeAssistant.Domain.Entities;

namespace PersonalLifeAssistant.Application.Common.Interfaces;

public interface IChatHistoryRepository : IRepository<ChatHistory>
{
    Task<IReadOnlyList<ChatHistory>> GetRecentByUserAsync(Guid userId, int limit, CancellationToken cancellationToken = default);
}
