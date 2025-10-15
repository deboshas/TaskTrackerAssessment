using ErrorOr;
using Microsoft.Extensions.Logging;
using TaskTracker.Application.CQRS.Abstractions;
using TaskTracker.Application.Extensions;
using TaskTracker.Contracts.Response.Task;
using TaskTracker.Infrastructure.Repositories.Abstractions;

namespace TaskTracker.Application.Task.Search;

/// <summary>
/// Handles the query for searching tasks based on specific criteria.
/// </summary>
public sealed class SearchTasksQueryHandler : IQueryHandler<SearchTasksQuery, ErrorOr<List<TaskResponse>>>
{
    private readonly ITaskTrackerRepository _taskTrackerRepository;
    private readonly ILogger<SearchTasksQueryHandler> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="SearchTasksQueryHandler"/> class.
    /// </summary>
    /// <param name="taskTrackerRepository">The repository for accessing task data.</param>
    /// <param name="logger">The logger instance for logging information.</param>
    /// <exception cref="ArgumentNullException">Thrown when any dependency is null.</exception>
    public SearchTasksQueryHandler(
        ITaskTrackerRepository taskTrackerRepository,
        ILogger<SearchTasksQueryHandler> logger)
    {
        _taskTrackerRepository = taskTrackerRepository ?? throw new ArgumentNullException(nameof(taskTrackerRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Handles the search query and retrieves tasks matching the specified criteria.
    /// </summary>
    /// <param name="searchTasksQuery">The query containing search criteria.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A list of tasks matching the criteria or an error.</returns>
    public async Task<ErrorOr<List<TaskResponse>>> Handle(
        SearchTasksQuery searchTasksQuery,
        CancellationToken cancellationToken)
    {
        try
        {
            var tasks = await _taskTrackerRepository.QueryAsync();

            if (tasks is null || !tasks.Any())
            {
                return Error.NotFound("No tasks found in the db!");
            }

            var filteredTasks = tasks
                                .WithTitle(searchTasksQuery.SearchRequest.Title)
                                .WithDescription(searchTasksQuery.SearchRequest.Description)
                                .WithStatus(searchTasksQuery.SearchRequest.Status)
                                .WithPriority(searchTasksQuery.SearchRequest.Priority)
                                .WithDueDate(searchTasksQuery.SearchRequest.DueDate);

            if (!filteredTasks.Any())
            {
                return Error.NotFound("No tasks match the search criteria!");
            }

            return filteredTasks.MapToResponse().ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving tasks.");
            return Error.Failure(
                "An error occurred while retrieving tasks. Please try again later.");
        }
    }
}
