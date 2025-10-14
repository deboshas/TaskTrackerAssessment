namespace TaskTracker.Contracts.Request.Task;

public record CreateTaskRequest(
   string Title,
   string? Description,
   string Status,
   DateTime? DueDate,
   string Priority,
   string UserId
);
