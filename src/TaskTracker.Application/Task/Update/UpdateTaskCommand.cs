using ErrorOr;
using TaskTracker.Application.CQRS.Abstractions;
using TaskTracker.Contracts.Request.Task;

namespace TaskTracker.Application.Task.Update;
public record UpdateTaskCommand(UpdateTaskRequest UpdateTaskRequest) : ICommand<ErrorOr<Success>>;
