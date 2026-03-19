using Microsoft.EntityFrameworkCore;
using PersonalLifeAssistant.Application.Common.Interfaces;

namespace PersonalLifeAssistant.Infrastructure.Persistence.Repositories;

public class EfRepository<T> : IRepository<T> where T : class
{
    protected readonly ApplicationDbContext DbContext;

    public EfRepository(ApplicationDbContext dbContext)
    {
        DbContext = dbContext;
    }

    public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await DbContext.Set<T>().FindAsync(new object?[] { id }, cancellationToken);

    public async Task<IReadOnlyList<T>> ListAsync(CancellationToken cancellationToken = default)
        => await DbContext.Set<T>().ToListAsync(cancellationToken);

    public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
        => await DbContext.Set<T>().AddAsync(entity, cancellationToken);

    public void Update(T entity) => DbContext.Set<T>().Update(entity);

    public void Remove(T entity) => DbContext.Set<T>().Remove(entity);
}
