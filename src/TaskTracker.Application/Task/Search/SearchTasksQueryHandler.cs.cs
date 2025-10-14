using ErrorOr;
using Microsoft.Extensions.Logging;
using TaskTracker.Application.CQRS.Abstractions;
using TaskTracker.Contracts.Request.Task;
using TaskTracker.Contracts.Response;
using TaskTracker.Infrastructure.Repositories.Abstractions;

namespace TaskTracker.Application.Task.Search;

public class SearchTasksQueryHandler : IQueryHandler<SearchTasksQuery, ErrorOr<List<TaskResponse>>>
{
    private readonly ITaskTrackerRepository _taskTrackerRepository;
    private readonly ILogger<SearchTasksQueryHandler> _logger;

    public SearchTasksQueryHandler(ITaskTrackerRepository taskTrackerRepository, ILogger<SearchTasksQueryHandler> logger)
    {

        _taskTrackerRepository = taskTrackerRepository ?? throw new ArgumentNullException(nameof(taskTrackerRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    public async Task<ErrorOr<List<TaskResponse>>> Handle(SearchTasksQuery searchTasksQuery, CancellationToken cancellationToken)
    {
        try
        {
            var tasks = await _taskTrackerRepository.QueryAsync();

            if (tasks == null || !tasks.Any())
            {
                return Error.NotFound("No tasks found in the db!");
            }

            var filteredTasks = tasks.Where(task =>
                               (string.IsNullOrEmpty(searchTasksQuery.SearchRequest.Title) || task.Title.Contains(searchTasksQuery.SearchRequest.Title, StringComparison.OrdinalIgnoreCase)) &&
                               (string.IsNullOrEmpty(searchTasksQuery.SearchRequest.Description) || task.Description.Contains(searchTasksQuery.SearchRequest.Description, StringComparison.OrdinalIgnoreCase)) &&
                                (searchTasksQuery.SearchRequest.Status == null || Enum.TryParse(searchTasksQuery.SearchRequest.Status, ignoreCase: true, out Status status) && task.Status.Equals(status)) &&
                                (searchTasksQuery.SearchRequest.Priority == null || Enum.TryParse<Status>(searchTasksQuery.SearchRequest.Priority, ignoreCase: true, out var priority) && task.Status.Equals(priority)



            if (!filteredTasks.Any())
            {
                return Error.NotFound("No tasks match the search criteria!");
            }

            return filteredTasks.Select(task => new TaskResponse
            {
                TaskId = task.Id,
                Title = task.Title,
                Description = task.Description,
                Status = task.Status,
                UserId = task.UserId,
                Priority = task.Priority,
                DueDate = task.Status.ToString()

            }).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving tasks.");
            return Error.Failure("An error occurred while retrieving tasks. Please try again later.");
        }
    }
}
