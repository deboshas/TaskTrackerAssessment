using ErrorOr;
using TaskTracker.Application.CQRS.Abstractions;
using TaskTracker.Contracts.Request.Task;
using TaskTracker.Contracts.Response;

namespace TaskTracker.Application.Task.Create;
public record CreateTaskCommand(CreateTaskRequest CreateTaskRequest) : ICommand<ErrorOr<Success>>;
