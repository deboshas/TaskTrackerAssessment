using TaskTracker.Application.Task.Create;
using TaskTracker.Application.Task.Update;
using TaskTracker.Domain.Task;

namespace TaskTracker.Application.Extensions;

public static class CommandExtensions
{
    /// <summary>  
    /// Maps a CreateTaskCommand object to a TaskItem domain object.  
    /// </summary>  
    /// <param name="createTaskCommand">The command containing task creation details.</param>  
    /// <returns>A TaskItem object populated with the data from the CreateTaskCommand.</returns>  
    public static TaskItem MapToDomain(this CreateTaskCommand createTaskCommand)
    {
        return new TaskItem
        {
            Title = createTaskCommand?.CreateTaskRequest?.Title,
            Description = createTaskCommand?.CreateTaskRequest?.Description,
            Status = (Status)createTaskCommand?.CreateTaskRequest?.Status,
            Priority = (Priority)createTaskCommand?.CreateTaskRequest?.Priority,
            DueDate = createTaskCommand?.CreateTaskRequest?.DueDate,
            UserId = createTaskCommand?.CreateTaskRequest?.UserId
        };
    }

    /// <summary>  
    /// Updates an existing TaskItem domain object with data from an UpdateTaskCommand.  
    /// </summary>  
    /// <param name="updateTaskCommand">The command containing updated task details.</param>  
    /// <param name="taskToUpdate">The existing TaskItem to be updated.</param>  
    /// <returns>The updated TaskItem object.</returns>  
    /// <exception cref="ArgumentNullException">Thrown if the taskToUpdate parameter is null.</exception>  
    public static TaskItem MapUpdatedTask(this UpdateTaskCommand updateTaskCommand, TaskItem taskToUpdate)
    {
        if (taskToUpdate == null)
        {
            throw new ArgumentNullException(nameof(taskToUpdate));
        }
     
        taskToUpdate.Title = updateTaskCommand.UpdateTaskRequest.Title;
        taskToUpdate.Description = updateTaskCommand.UpdateTaskRequest.Description;
        taskToUpdate.Status = (Domain.Task.Status)updateTaskCommand.UpdateTaskRequest.Status;
        taskToUpdate.Priority = (Domain.Task.Priority)updateTaskCommand.UpdateTaskRequest.Priority;
        taskToUpdate.DueDate = updateTaskCommand.UpdateTaskRequest.DueDate;
        taskToUpdate.UserId = updateTaskCommand.UpdateTaskRequest.UserId;

        return taskToUpdate;
    }
}
