using TaskTracker.Domain.Task;
using TaskTracker.Kernel.Common;

namespace TaskTracker.Infrastructure.Repositories.Abstractions;

public  interface ITaskTrackerRepository: IRepository<TaskItem> { }
