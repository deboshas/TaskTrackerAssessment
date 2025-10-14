using ErrorOr;
using TaskTracker.Application.CQRS.Abstractions;
using TaskTracker.Contracts.Response.Task;

namespace TaskTracker.Application.Task.GetAll;
public record GetAllTasksQuery : IQuery<ErrorOr<List<TaskResponse>>>
{
}
