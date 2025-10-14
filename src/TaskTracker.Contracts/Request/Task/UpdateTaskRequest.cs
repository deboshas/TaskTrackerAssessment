namespace TaskTracker.Contracts.Request.Task;

public record UpdateTaskRequest(Guid Id,
   string Title,
   string? Description,
   string Status,
   DateTime? DueDate,
   string Priority,
   string UserId);
