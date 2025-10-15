using TaskTracker.Contracts.Common;

namespace TaskTracker.Contracts.Response.Task;

public record TaskResponse(
    Guid TaskId,
    string Title,
    string? Description,
    string Status,
    DateTime? DueDate,
    string Priority,
    string UserId
);

