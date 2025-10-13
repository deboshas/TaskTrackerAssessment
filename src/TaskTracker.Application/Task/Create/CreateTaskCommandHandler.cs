using ErrorOr;
using Microsoft.Extensions.Logging;
using TaskTracker.Application.CQRS.Abstractions;
using TaskTracker.Application.Extensions;
using TaskTracker.Domain.Task;
using TaskTracker.Infrastructure.Repositories.Abstractions;

namespace TaskTracker.Application.Task.Create;

public class CreateTaskCommandHandler : ICommandHander<CreateTaskCommand, ErrorOr<Success>>
{
    private readonly ITaskTrackerRepository _taskTrackerRepository;
    private readonly ILogger<CreateTaskCommandHandler> _logger;

    public CreateTaskCommandHandler(ITaskTrackerRepository taskTrackerRepository, ILogger<CreateTaskCommandHandler> logger)
    {

        _taskTrackerRepository = taskTrackerRepository ?? throw new ArgumentNullException(nameof(taskTrackerRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<ErrorOr<Success>> Handle(CreateTaskCommand createTaskCommand, CancellationToken cancellationToken)
    {
        try
        {
            _taskTrackerRepository.Add(createTaskCommand.MapToDomain());
            await _taskTrackerRepository.SaveChangesAsync(cancellationToken);
            return new Success();

        }

        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating task for UserId: {UserId}", createTaskCommand.CreateTaskRequest.UserId);
            return Error.Failure(description: "An error occurred while creating the task. Please try again later.");
        }
    }
}
