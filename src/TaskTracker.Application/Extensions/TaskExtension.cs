using TaskTracker.Domain.Task;

namespace TaskTracker.Application.Extensions;

/// <summary>
/// Provides extension methods for building and filtering task search queries.
/// </summary>
public static class TaskExtension
{

    /// <summary>
    /// Filters tasks by title (case-insensitive).
    /// </summary>
    /// <param name="tasks">The queryable collection of tasks.</param>
    /// <param name="title">The title to filter by.</param>
    /// <returns>The filtered queryable collection.</returns>
    public static IQueryable<TaskItem> WithTitle(this IQueryable<TaskItem> tasks, string? title)
    {
        if (!string.IsNullOrWhiteSpace(title))
        {
            var loweredTitle = title.ToLower();
            return tasks.Where(task => task.Title.ToLower().Contains(loweredTitle));
        }
        return tasks;
    }

    /// <summary>
    /// Filters tasks by description (case-insensitive).
    /// </summary>
    /// <param name="tasks">The queryable collection of tasks.</param>
    /// <param name="description">The description to filter by.</param>
    /// <returns>The filtered queryable collection.</returns>
    public static IQueryable<TaskItem> WithDescription(this IQueryable<TaskItem> tasks, string? description)
    {
        if (!string.IsNullOrWhiteSpace(description))
        {
            var loweredDescription = description.ToLower();
            return tasks.Where(task => task.Description != null && task.Description.ToLower().Contains(loweredDescription));
        }
        return tasks;
    }
}