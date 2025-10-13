using System.Diagnostics.CodeAnalysis;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskTracker.Application.Task.Create;
using TaskTracker.Application.Task.GetAll;
using TaskTracker.Application.Task.Remove;
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

    [HttpGet("all")]
    public async Task<IActionResult> GetAllTasks()
    {
        var result = await _sender.Send(new GetAllTasksQuery());
        return result.Match(
            tasks => Ok(tasks),
            Problem);
    }

    [HttpPost("add")]
    public async Task<IActionResult> CreateTask([FromBody] CreateTaskRequest createTaskRequest)
    {

        var createtaskCommand = new CreateTaskCommand(createTaskRequest);
        var result = await _sender.Send(createtaskCommand);
        return result.Match(
            task => Created(task),
            Problem);

    }

    [HttpPut("update")]
    public async Task<IActionResult> Updatetask([FromBody] UpdateTaskRequest updateTaskRequest)
    {
        var updateTaskCommand = new UpdateTaskCommand(updateTaskRequest);
        var result = await _sender.Send(updateTaskCommand);
        return result.Match(
            task => Ok(task),
            Problem);

    }

    [HttpDelete("remove/{taskId}")]
    public async Task<IActionResult> Updatetask(Guid taskId)
    {
        var removeTaskCommand = new RemoveTaskCommand(taskId);
        var result = await _sender.Send(removeTaskCommand);
        return result.Match(
            _ => Ok(),
            Problem);

    }
}
