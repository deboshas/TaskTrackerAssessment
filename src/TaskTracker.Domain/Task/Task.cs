namespace TaskTracker.Domain.Task;

public class TaskItem
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public Status Status { get; set; }
    public DateTime? DueDate { get; set; }
    public Priority Priority { get; set; }
    public required string UserId { get; set; }
}
