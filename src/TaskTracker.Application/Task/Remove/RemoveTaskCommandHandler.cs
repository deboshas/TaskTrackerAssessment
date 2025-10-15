using ErrorOr;
using Microsoft.Extensions.Logging;
using TaskTracker.Application.CQRS.Abstractions;
using TaskTracker.Infrastructure.Repositories.Abstractions;

namespace TaskTracker.Application.Task.Remove;

public class RemoveTaskCommandHandler : ICommandHander<RemoveTaskCommand, ErrorOr<Success>>
{
    private readonly ITaskTrackerRepository _taskTrackerRepository;
    private readonly ILogger<RemoveTaskCommandHandler> _logger;

    public RemoveTaskCommandHandler(ITaskTrackerRepository taskTrackerRepository, ILogger<RemoveTaskCommandHandler> logger)
    {

        _taskTrackerRepository = taskTrackerRepository ?? throw new ArgumentNullException(nameof(taskTrackerRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<ErrorOr<Success>> Handle(RemoveTaskCommand removeTaskCommand, CancellationToken cancellationToken)
    {
        try
        {
            Guid.TryParse(removeTaskCommand.TaskId, out var taskId);
           
            var taskToRemove = await _taskTrackerRepository.GetByIdAsync(taskId, cancellationToken);

            if (taskToRemove == null)
            {
                return Error.NotFound(description: $"Task with Id: {removeTaskCommand.TaskId} not found.");
            }

            _taskTrackerRepository.Delete(taskToRemove);
            await _taskTrackerRepository.SaveChangesAsync(cancellationToken);
            return new Success();
        }

        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error occurred while removing task with task id: {removeTaskCommand.TaskId}");
            return Error.Failure(description: "An error occurred while removing the task. Please try again later.");
        }
    }
}
