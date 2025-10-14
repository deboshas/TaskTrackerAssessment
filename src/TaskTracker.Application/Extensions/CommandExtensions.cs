using TaskTracker.Application.Task.Create;
using TaskTracker.Application.Task.Update;
using TaskTracker.Contracts.Common;
using TaskTracker.Contracts.Response.Task;
using TaskTracker.Domain.Task;

namespace TaskTracker.Application.Extensions;

public static class CommandExtensions
{
    /// <summary>  
    /// Maps a CreateTaskCommand object to a TaskItem domain object.  
    /// </summary>  
    /// <param name="createTaskCommand">The command containing task creation details.</param>  
    /// <returns>A TaskItem object populated with the data from the CreateTaskCommand.</returns>  
    internal static TaskItem MapToDomain(this CreateTaskCommand createTaskCommand)
    {
        return new TaskItem
        {
            Title = createTaskCommand?.CreateTaskRequest?.Title ?? throw new ArgumentNullException(nameof(createTaskCommand.CreateTaskRequest.Title)),
            Description = createTaskCommand?.CreateTaskRequest?.Description,
            Status = Enum.TryParse<Domain.Task.Status>(createTaskCommand?.CreateTaskRequest?.Status, ignoreCase: true, out var status) ? status : throw new ArgumentException("Invalid Status value"),
            Priority = Enum.TryParse<Domain.Task.Priority>(createTaskCommand?.CreateTaskRequest?.Priority, ignoreCase: true, out var priority) ? priority : throw new ArgumentException("Invalid Priority value"),
            DueDate = createTaskCommand?.CreateTaskRequest?.DueDate,
            UserId = createTaskCommand?.CreateTaskRequest?.UserId ?? throw new ArgumentNullException(nameof(createTaskCommand.CreateTaskRequest.UserId))
        };
    }

    /// <summary>  
    /// Updates an existing TaskItem domain object with data from an UpdateTaskCommand.  
    /// </summary>  
    /// <param name="updateTaskCommand">The command containing updated task details.</param>  
    /// <param name="taskToUpdate">The existing TaskItem to be updated.</param>  
    /// <returns>The updated TaskItem object.</returns>  
    /// <exception cref="ArgumentNullException">Thrown if the taskToUpdate parameter is null.</exception>  
    internal static TaskItem MapUpdatedTask(this UpdateTaskCommand updateTaskCommand, TaskItem taskToUpdate)
    {
        if (taskToUpdate == null)
        {
            throw new ArgumentNullException(nameof(taskToUpdate));
        }

        // Update the properties of the existing TaskItem with the values from the UpdateTaskCommand.  
        taskToUpdate.Title = updateTaskCommand.UpdateTaskRequest.Title;
        taskToUpdate.Description = updateTaskCommand.UpdateTaskRequest.Description;
        taskToUpdate.Status = Enum.TryParse<Domain.Task.Status>(updateTaskCommand.UpdateTaskRequest.Status, ignoreCase: true, out var status) ? status : throw new ArgumentException("Invalid Status value");
        taskToUpdate.Priority = Enum.TryParse<Domain.Task.Priority>(updateTaskCommand.UpdateTaskRequest.Priority, ignoreCase: true, out var priority) ? priority : throw new ArgumentException("Invalid Priority value");
        taskToUpdate.DueDate = updateTaskCommand.UpdateTaskRequest.DueDate;
        taskToUpdate.UserId = updateTaskCommand.UpdateTaskRequest.UserId;

        return taskToUpdate;
    }

    internal static IEnumerable<TaskResponse> MapToResponse(this IQueryable<TaskItem> filteredTasks)
    {
        foreach (var filteredTask in filteredTasks)
        {
            yield return new TaskResponse(
                filteredTask.Id,
                filteredTask.Title,
                filteredTask.Description,
                (Contracts.Common.Status)filteredTask.Status,
                filteredTask.DueDate,
                (Contracts.Common.Priority)filteredTask.Priority,
                filteredTask.UserId
            );
        }
    }
}
