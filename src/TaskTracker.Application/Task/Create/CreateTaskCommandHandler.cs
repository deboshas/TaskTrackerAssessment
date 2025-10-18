using ErrorOr;
using Microsoft.Extensions.Logging;
using TaskTracker.Application.CQRS.Abstractions;
using TaskTracker.Application.Extensions;
using TaskTracker.Contracts.Response.Task;
using TaskTracker.Infrastructure.Repositories.Abstractions;

namespace TaskTracker.Application.Task.Create;

/// <summary>  
/// Handles the creation of tasks by processing the CreateTaskCommand.  
/// </summary>  
public class CreateTaskCommandHandler : ICommandHander<CreateTaskCommand, ErrorOr<TaskResponse>>
{
    private readonly ITaskTrackerRepository _taskTrackerRepository;
    private readonly ILogger<CreateTaskCommandHandler> _logger;

    /// <summary>  
    /// Initializes a new instance of the <see cref="CreateTaskCommandHandler"/> class.  
    /// </summary>  
    /// <param name="taskTrackerRepository">The repository for managing task data.</param>  
    /// <param name="logger">The logger for logging errors and information.</param>  
    /// <exception cref="ArgumentNullException">Thrown when taskTrackerRepository or logger is null.</exception>  
    public CreateTaskCommandHandler(ITaskTrackerRepository taskTrackerRepository, ILogger<CreateTaskCommandHandler> logger)
    {
        _taskTrackerRepository = taskTrackerRepository ?? throw new ArgumentNullException(nameof(taskTrackerRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>  
    /// Handles the CreateTaskCommand to create a new task.  
    /// </summary>  
    /// <param name="createTaskCommand">The command containing task creation details.</param>  
    /// <param name="cancellationToken">A token to cancel the operation.</param>  
    /// <returns>An ErrorOr result indicating success or failure.</returns>  
    public async Task<ErrorOr<TaskResponse>> Handle(CreateTaskCommand createTaskCommand, CancellationToken cancellationToken)
    {
        try
        {
            // Create the domain entity
            var taskEntity = createTaskCommand.MapToDomain();

            // Add to repository
            _taskTrackerRepository.Add(taskEntity);
       

            // Save changes to the repository asynchronously
            await _taskTrackerRepository.SaveChangesAsync(cancellationToken);

            // Fetch the created task using its ID
            var createdTask = await _taskTrackerRepository.GetByIdAsync(taskEntity.Id, cancellationToken);

            if (createdTask is null)
            {
                return Error.Unexpected(description: "Created task could not be retrieved");
            }

            // Map to response and return
            return createdTask.MapToResponse();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating task for UserId: {UserId}", createTaskCommand.CreateTaskRequest.UserId);
            return Error.Failure(description: "An error occurred while creating the task. Please try again later.");
        }
    }
}
