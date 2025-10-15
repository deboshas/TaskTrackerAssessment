using ErrorOr;
using TaskTracker.Application.CQRS.Abstractions;

namespace TaskTracker.Application.Task.Remove;
public record RemoveTaskCommand(string TaskId) : ICommand<ErrorOr<Success>>;
