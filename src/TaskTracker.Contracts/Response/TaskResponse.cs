using TaskTracker.Contracts.Request.Task;

namespace TaskTracker.Contracts.Response;

public record TaskResponse(
    Guid TaskId,
    string Title,
    string? Description,
    Status Status,
    DateTime? DueDate,
    Priority Priority,
    string UserId
);

