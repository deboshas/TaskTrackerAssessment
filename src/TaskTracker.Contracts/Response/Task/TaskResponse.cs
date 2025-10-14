using TaskTracker.Contracts.Common;

namespace TaskTracker.Contracts.Response.Task;

public record TaskResponse(
    Guid TaskId,
    string Title,
    string? Description,
    Status Status,
    DateTime? DueDate,
    Priority Priority,
    string UserId
);

