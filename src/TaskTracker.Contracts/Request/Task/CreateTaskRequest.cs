namespace TaskTracker.Contracts.Request.Task;

public record CreateTaskRequest(
   string Title,
   string? Description,
   Status Status,
   DateTime? DueDate,
   Priority Priority,
   string UserId
);
