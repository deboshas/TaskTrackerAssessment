namespace TaskTracker.Contracts.Request.Task;

public record UpdateTaskRequest(Guid Id,
   string Title,
   string? Description,
   Status Status,
   DateTime? DueDate,
   Priority Priority,
   string UserId);
