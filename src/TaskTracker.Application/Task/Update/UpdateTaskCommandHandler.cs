using ErrorOr;
using Microsoft.Extensions.Logging;
using TaskTracker.Application.CQRS.Abstractions;
using TaskTracker.Application.Extensions;
using TaskTracker.Application.Task.Create;
using TaskTracker.Infrastructure.Repositories.Abstractions;

namespace TaskTracker.Application.Task.Update;

public class UpdateTaskCommandHandler : ICommandHander<UpdateTaskCommand, ErrorOr<Success>>
{
    private readonly ITaskTrackerRepository _taskTrackerRepository;
    private readonly ILogger<CreateTaskCommandHandler> _logger;

    public UpdateTaskCommandHandler(ITaskTrackerRepository taskTrackerRepository, ILogger<CreateTaskCommandHandler> logger)
    {

        _taskTrackerRepository = taskTrackerRepository ?? throw new ArgumentNullException(nameof(taskTrackerRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<ErrorOr<Success>> Handle(UpdateTaskCommand updateTaskCommand, CancellationToken cancellationToken)
    {
        try
        {
            var taskToUpdate = await _taskTrackerRepository.GetByIdAsync(updateTaskCommand.UpdateTaskRequest.Id, cancellationToken);
            if (taskToUpdate == null)
            {
                return Error.NotFound(description: $"Task with Id: {updateTaskCommand.UpdateTaskRequest.Id} not found.");
            }

            _taskTrackerRepository.Update(updateTaskCommand.MapUpdatedTask(taskToUpdate));
            await _taskTrackerRepository.SaveChangesAsync(cancellationToken);
            return new Success();
        }

        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error occurred while removing task with task id: {updateTaskCommand.UpdateTaskRequest.Id}");
            return Error.Failure(description: "An error occurred while removing the task. Please try again later.");
        }
    }
}
