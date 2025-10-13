using ErrorOr;
using Microsoft.Extensions.Logging;
using TaskTracker.Application.CQRS.Abstractions;
using TaskTracker.Domain.Task;
using TaskTracker.Infrastructure.Repositories.Abstractions;

namespace TaskTracker.Application.Task.Create
{
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
                var taskItem = new TaskItem()
                {
                    Title = createTaskCommand.CreateTaskRequest.Title,
                    Description = createTaskCommand.CreateTaskRequest.Description,
                    UserId = createTaskCommand.CreateTaskRequest.UserId,
                    Status = (Status)createTaskCommand.CreateTaskRequest.Status,
                    Priority = (Priority)createTaskCommand.CreateTaskRequest.Priority,
                    DueDate = createTaskCommand.CreateTaskRequest.DueDate

                };

                _taskTrackerRepository.Add(taskItem);
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
}
