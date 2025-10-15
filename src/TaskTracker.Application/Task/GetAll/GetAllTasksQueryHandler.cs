using ErrorOr;
using Microsoft.Extensions.Logging;
using TaskTracker.Application.CQRS.Abstractions;
using TaskTracker.Application.Extensions;
using TaskTracker.Contracts.Common;
using TaskTracker.Contracts.Response.Task;
using TaskTracker.Infrastructure.Repositories.Abstractions;

namespace TaskTracker.Application.Task.GetAll;

public class GetAllTasksQueryHandler : IQueryHandler<GetAllTasksQuery, ErrorOr<List<TaskResponse>>>
{
    private readonly ITaskTrackerRepository _taskTrackerRepository;
    private readonly ILogger<GetAllTasksQueryHandler> _logger;

    public GetAllTasksQueryHandler(ITaskTrackerRepository taskTrackerRepository, ILogger<GetAllTasksQueryHandler> logger)
    {

        _taskTrackerRepository = taskTrackerRepository ?? throw new ArgumentNullException(nameof(taskTrackerRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    public async Task<ErrorOr<List<TaskResponse>>> Handle(GetAllTasksQuery getAllTasksQuery, CancellationToken cancellationToken)
    {
        try
        {
            var tasks = await _taskTrackerRepository.QueryAsync();

            if (tasks == null || !tasks.Any())
            {
                return Error.NotFound("No tasks found!");
            }

            return tasks.MapToResponse().ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving tasks.");
            return Error.Failure("An error occurred while retrieving tasks. Please try again later.");
        }
    }
}
