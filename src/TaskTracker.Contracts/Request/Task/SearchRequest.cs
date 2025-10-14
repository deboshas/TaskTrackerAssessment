namespace TaskTracker.Contracts.Request.Task;

public record SearchRequest(
   string Title="",
   string? Description = "",
   string? Status = "",
   DateTime? DueDate=null,
   string? Priority = "",
   string? UserId = "");
