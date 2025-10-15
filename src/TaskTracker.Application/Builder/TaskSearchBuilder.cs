using System.Linq;
using TaskTracker.Domain.Task;

namespace TaskTracker.Application.Builder;

/// <summary>
/// Builder class for constructing task search queries
/// </summary>
public static class TaskSearchBuilder
{   
    /// <summary>
    /// Builds and returns the filtered query
    /// </summary>
    public static IQueryable<TaskItem> Build(IQueryable<TaskItem> tasks)
    {
        if (tasks == null)
            throw new InvalidOperationException("TaskSearchBuilder must be initialized before use");
            
        return tasks;
    }

    /// <summary>
    /// Filters tasks by title
    /// </summary>
    public static IQueryable<TaskItem> WithTitle(this IQueryable<TaskItem> tasks, string? title)
    {

        if (!string.IsNullOrEmpty(title))
        {
            return tasks.Where(task =>
                task.Title.ToLower().Contains(title.ToLower()));
        }
        return tasks;
    }

    /// <summary>
    /// Filters tasks by description
    /// </summary>
    public static IQueryable<TaskItem> WithDescription(this IQueryable<TaskItem> tasks, string? description)
    {       
        if (!string.IsNullOrEmpty(description))
        {
            return tasks.Where(task =>
                task.Description != null &&
                task.Description.ToLower().Contains(description.ToLower()));
        }
        return tasks;
    }
}