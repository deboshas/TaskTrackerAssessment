using ErrorOr;
using TaskTracker.Application.CQRS.Abstractions;
using TaskTracker.Contracts.Request.Task;
using TaskTracker.Contracts.Response.Task;

namespace TaskTracker.Application.Task.Search;
public record SearchTasksQuery(SearchRequest SearchRequest) : IQuery<ErrorOr<List<TaskResponse>>>;

