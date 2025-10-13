namespace TaskTracker.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using TaskTracker.Domain.Task;

public class TaskTrackerDbContext : DbContext
{
    public TaskTrackerDbContext(DbContextOptions<TaskTrackerDbContext> options) : base(options) { }
    public DbSet<TaskItem> Tasks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("tasktracker");
    }

    public override int SaveChanges()
    {
        ChangeTracker.CascadeDeleteTiming=Microsoft.EntityFrameworkCore.ChangeTracking.CascadeTiming.Never;
        ChangeTracker.DeleteOrphansTiming = Microsoft.EntityFrameworkCore.ChangeTracking.CascadeTiming.Never;
        ChangeTracker.DetectChanges();
        return base.SaveChanges();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        ChangeTracker.CascadeDeleteTiming = Microsoft.EntityFrameworkCore.ChangeTracking.CascadeTiming.Never;
        ChangeTracker.DeleteOrphansTiming = Microsoft.EntityFrameworkCore.ChangeTracking.CascadeTiming.Never;
        ChangeTracker.DetectChanges();
        return await base.SaveChangesAsync(cancellationToken);
    }
}
