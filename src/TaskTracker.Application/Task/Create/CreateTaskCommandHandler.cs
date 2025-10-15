using ErrorOr;
using Microsoft.Extensions.Logging;
using TaskTracker.Application.CQRS.Abstractions;
using TaskTracker.Application.Extensions;
using TaskTracker.Infrastructure.Repositories.Abstractions;

namespace TaskTracker.Application.Task.Create;

/// <summary>  
/// Handles the creation of tasks by processing the CreateTaskCommand.  
/// </summary>  
public class CreateTaskCommandHandler : ICommandHander<CreateTaskCommand, ErrorOr<Success>>
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
    public async Task<ErrorOr<Success>> Handle(CreateTaskCommand createTaskCommand, CancellationToken cancellationToken)
    {
        try
        {
            // Map the command to a domain task entity and add it to the repository.  
            _taskTrackerRepository.Add(createTaskCommand.MapToDomain());

            // Save changes to the repository asynchronously.  
            await _taskTrackerRepository.SaveChangesAsync(cancellationToken);

            // Return success if the operation completes without errors.  
            return new Success();
        }
        catch (Exception ex)
        {
            // Log the error and return a failure result with a descriptive message.  
            _logger.LogError(ex, "Error occurred while creating task for UserId: {UserId}", createTaskCommand.CreateTaskRequest.UserId);
            return Error.Failure(description: "An error occurred while creating the task. Please try again later.");
        }
    }
}
