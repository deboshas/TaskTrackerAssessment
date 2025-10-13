using System.Diagnostics.CodeAnalysis;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskTracker.Application.Task.Create;
using TaskTracker.Application.Task.GetAll;
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
}
