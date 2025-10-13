namespace TaskTracker.Contracts.Response;

public record CreateTaskResponse(int taskId, string userId,string errorMessage);
