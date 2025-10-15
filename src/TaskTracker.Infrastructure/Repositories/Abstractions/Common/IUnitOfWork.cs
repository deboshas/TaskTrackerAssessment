namespace TaskTracker.Infrastructure.Repositories.Abstractions.Common
{
    public interface IUnitOfWork
    {
        int SaveChanges();

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
