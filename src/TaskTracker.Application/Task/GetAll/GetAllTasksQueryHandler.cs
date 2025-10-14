using ErrorOr;
using Microsoft.Extensions.Logging;
using TaskTracker.Application.CQRS.Abstractions;
using TaskTracker.Contracts.Response;
using TaskTracker.Infrastructure.Repositories.Abstractions;

namespace TaskTracker.Application.Task.GetAll;

public class GetAllTasksQueryHandler : IQueryHandler<SearchTasksQuery, ErrorOr<List<TaskResponse>>>
{
    private readonly ITaskTrackerRepository _taskTrackerRepository;
    private readonly ILogger<GetAllTasksQueryHandler> _logger;

    public GetAllTasksQueryHandler(ITaskTrackerRepository taskTrackerRepository, ILogger<GetAllTasksQueryHandler> logger)
    {

        _taskTrackerRepository = taskTrackerRepository ?? throw new ArgumentNullException(nameof(taskTrackerRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    public async Task<ErrorOr<List<TaskResponse>>> Handle(SearchTasksQuery getAllTasksQuery, CancellationToken cancellationToken)
    {
        try
        {
            var tasks = await _taskTrackerRepository.QueryAsync();

            if (tasks == null || !tasks.Any())
            {
                return Error.NotFound("No tasks found!");
            }

            return tasks.ToList()
                .Select(t => new TaskResponse(
                    t.Id,
                    t.Title,
                    t.Description,
                    (Contracts.Request.Task.Status)t.Status,
                    t.DueDate,
                    (Contracts.Request.Task.Priority)t.Priority,
                    t.UserId
                )).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving tasks.");
            return Error.Failure("An error occurred while retrieving tasks. Please try again later.");
        }
    }
}
