using TaskTracker.Domain.Task;
using TaskTracker.Infrastructure.Repositories.Abstractions.Common;

namespace TaskTracker.Infrastructure.Repositories.Abstractions;

public interface ITaskTrackerRepository : IRepository<TaskItem>
{
}
