using System.Diagnostics.CodeAnalysis;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskTracker.Application.Task.Create;
using TaskTracker.Application.Task.GetAll;
using TaskTracker.Application.Task.Remove;
using TaskTracker.Application.Task.Search;
using TaskTracker.Application.Task.Update;
using TaskTracker.Contracts.Request.Task;

namespace TaskTracker.API.Controllers;


[ExcludeFromCodeCoverage]
[Route("tasks")]
public class TaskController : ApiController
{
    private readonly ISender _sender;

    public TaskController(ISender sender)
    {
        _sender = sender ?? throw new ArgumentNullException(nameof(sender));
    }

    /// <summary>  
    /// Retrieves all tasks from the system.  
    /// </summary>  
    /// <returns>An IActionResult containing the list of tasks or an error response.</returns>  
    [HttpGet("all")]
    public async Task<IActionResult> GetAllTasks()
    {
        var result = await _sender.Send(new GetAllTasksQuery());
        return result.Match(
            tasks => Ok(tasks),
            Problem);
    }

    /// <summary>  
    /// Searches for tasks based on the provided search criteria.  
    /// </summary>  
    /// <param name="searchRequest">The search criteria to filter tasks.</param>  
    /// <returns>An IActionResult containing the matching tasks or an error response.</returns>  
    [HttpPost("search")]
    public async Task<IActionResult> Search(SearchRequest searchRequest)
    {
        var searchTasksQuery = new SearchTasksQuery(searchRequest);
        var result = await _sender.Send(searchTasksQuery);
        return result.Match(
            _ => Ok(),
            Problem);
    }

    /// <summary>  
    /// Creates a new task based on the provided request data.  
    /// </summary>  
    /// <param name="createTaskRequest">The request object containing task details.</param>  
    /// <returns>An IActionResult containing the created task or an error response.</returns>  
    [HttpPost("add")]
    public async Task<IActionResult> CreateTask([FromBody] CreateTaskRequest createTaskRequest)
    {
        var createtaskCommand = new CreateTaskCommand(createTaskRequest);
        var result = await _sender.Send(createtaskCommand);
        return result.Match(
            task => Created(task),
            Problem);
    }

    /// <summary>  
    /// Updates an existing task based on the provided request data.  
    /// </summary>  
    /// <param name="updateTaskRequest">The request object containing updated task details.</param>  
    /// <returns>An IActionResult containing the updated task or an error response.</returns>  
    [HttpPut("update")]
    public async Task<IActionResult> Updatetask([FromBody] UpdateTaskRequest updateTaskRequest)
    {
        var updateTaskCommand = new UpdateTaskCommand(updateTaskRequest);
        var result = await _sender.Send(updateTaskCommand);
        return result.Match(
            task => Ok(task),
            Problem);
    }

    /// <summary>  
    /// Removes a task identified by the provided task ID.  
    /// </summary>  
    /// <param name="taskId">The unique identifier of the task to be removed.</param>  
    /// <returns>An IActionResult indicating success or an error response.</returns>  
    [HttpDelete("remove/{taskId}")]
    public async Task<IActionResult> Removetask(Guid taskId)
    {
        var removeTaskCommand = new RemoveTaskCommand(taskId);
        var result = await _sender.Send(removeTaskCommand);
        return result.Match(
            _ => Ok(),
            Problem);
    }

    
}
