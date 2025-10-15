using ErrorOr;
using TaskTracker.Application.CQRS.Abstractions;
using TaskTracker.Contracts.Request.Task;

namespace TaskTracker.Application.Task.Create;
/// <summary>  
/// Represents a command to create a new task.  
/// </summary>  
/// <param name="CreateTaskRequest">The request object containing details for the task to be created.</param>  
/// <returns>An <see cref="ErrorOr{T}"/> result containing either a success response or an error.</returns>  
public record CreateTaskCommand(CreateTaskRequest CreateTaskRequest) : ICommand<ErrorOr<Success>>;
