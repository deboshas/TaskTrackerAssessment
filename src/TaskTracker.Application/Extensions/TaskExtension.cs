using System;
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

    /// <summary>
    /// Filters tasks by status.
    /// </summary>
    /// <param name="tasks">The queryable collection of tasks.</param>
    /// <param name="status">The status to filter by.</param>
    /// <returns>The filtered queryable collection.</returns>
    public static IQueryable<TaskItem> WithStatus(this IQueryable<TaskItem> tasks, string? status)
    {
        if (!string.IsNullOrWhiteSpace(status) && Enum.TryParse<Status>(status, true, out var parsedStatus))
        {
            return tasks.Where(task => task.Status == parsedStatus);
        }
        return tasks;
    }

    /// <summary>
    /// Filters tasks by priority.
    /// </summary>
    /// <param name="tasks">The queryable collection of tasks.</param>
    /// <param name="priority">The priority to filter by.</param>
    /// <returns>The filtered queryable collection.</returns>
    public static IQueryable<TaskItem> WithPriority(this IQueryable<TaskItem> tasks, string? priority)
    {
        if (!string.IsNullOrWhiteSpace(priority) && Enum.TryParse<Priority>(priority, true, out var parsedPriority))
        {
            return tasks.Where(task => task.Priority == parsedPriority);
        }
        return tasks;
    }

    /// <summary>
    /// Filters tasks by user ID.
    /// </summary>
    /// <param name="tasks">The queryable collection of tasks.</param>
    /// <param name="userId">The user ID to filter by.</param>
    /// <returns>The filtered queryable collection.</returns>
    public static IQueryable<TaskItem> WithUserId(this IQueryable<TaskItem> tasks, string? userId)
    {
        if (!string.IsNullOrWhiteSpace(userId))
        {
            return tasks.Where(task => task.UserId == userId);
        }
        return tasks;
    }

    /// <summary>
    /// Filters tasks by specific due date.
    /// </summary>
    /// <param name="tasks">The queryable collection of tasks.</param>
    /// <param name="dueDate">The due date to filter by.</param>
    /// <returns>The filtered queryable collection.</returns>
    public static IQueryable<TaskItem> WithDueDate(this IQueryable<TaskItem> tasks, DateTime? dueDate)
    {
        if (dueDate.HasValue)
        {
            return tasks.Where(task => task.DueDate.Value.Year == dueDate.Value.Year
                              && task.DueDate.Value.Month == dueDate.Value.Month
                              && task.DueDate.Value.Day == dueDate.Value.Day);
        }
        return tasks;
    }
}