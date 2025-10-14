using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TaskTracker.Domain.Task;
using TaskTracker.Infrastructure.Persistance;
using TaskTracker.Infrastructure.Repositories.Abstractions;

namespace TaskTracker.Infrastructure.Repositories;

public class TaskTrackerRepository : ITaskTrackerRepository
{
    private readonly TaskTrackerDbContext _dbContext;
    private readonly ILogger<TaskTrackerRepository> _logger;

    public TaskTrackerRepository(TaskTrackerDbContext dbContext, ILogger<TaskTrackerRepository> logger)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public void Add(TaskItem entity)
    {
        _dbContext.Add(entity);
    }

    public void Add(IEnumerable<TaskItem> entities)
    {
        _dbContext.AddRange(entities);
    }

    public void Delete(TaskItem entity)
    {
        _dbContext.Remove(entity);
    }

    public void Delete(IEnumerable<TaskItem> entities)
    {
        _dbContext.RemoveRange(entities);
    }

    public void Update(TaskItem entity)
    {
        _dbContext.Update(entity);
    }

    public void Update(IEnumerable<TaskItem> entities)
    {
        _dbContext.UpdateRange(entities);
    }

    public Task<TaskItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return _dbContext.Tasks.AsNoTracking().FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }

    public IQueryable<TaskItem> Query()
    {
        return _dbContext.Tasks.AsQueryable();
    }

    public Task<IQueryable<TaskItem>> QueryAsync()
    {
        return Task.FromResult(_dbContext.Tasks.AsQueryable());
    }

    public int SaveChanges()
    {
        return _dbContext.SaveChanges();
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.SaveChangesAsync(cancellationToken);
    }
    
}
