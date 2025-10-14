using ErrorOr;
using Microsoft.Extensions.Logging;
using Moq;
using TaskTracker.Application.Task.Search;
using TaskTracker.Contracts.Request.Task;
using TaskTracker.Domain.Task;
using TaskTracker.Infrastructure.Repositories.Abstractions;

namespace TaskTracker.Application.Tests;

public class SearchTasksQueryHandlerTests
{
    private readonly Mock<ITaskTrackerRepository> _repositoryMock;
    private readonly Mock<ILogger<SearchTasksQueryHandler>> _loggerMock;
    private readonly SearchTasksQueryHandler _handler;

    public SearchTasksQueryHandlerTests()
    {
        _repositoryMock = new Mock<ITaskTrackerRepository>();
        _loggerMock = new Mock<ILogger<SearchTasksQueryHandler>>();
        _handler = new SearchTasksQueryHandler(_repositoryMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async void Handle_ReturnsMatchingTasks_WhenTasksMatchCriteria()
    {
        // Arrange
        var tasks = new List<TaskItem>
        {
            new TaskItem { Id = Guid.NewGuid(), Title = "Test", Description = "Desc", Status = TaskTracker.Domain.Task.Status.New, Priority = TaskTracker.Domain.Task.Priority.High, UserId = "user-123" },
            new TaskItem { Id = Guid.NewGuid(), Title = "Other", Description = "OtherDesc", Status = TaskTracker.Domain.Task.Status.Open, Priority = TaskTracker.Domain.Task.Priority.Low, UserId = "user-1234" }
        }.AsQueryable();

        _repositoryMock.Setup(r => r.QueryAsync())
            .ReturnsAsync(tasks);

        var searchRequest = new SearchRequest(Title: "Test",
            Description: "Desc",
            Status: "New",
            DueDate: DateTime.Now.AddDays(1),
            Priority: "High",
            UserId: "user-123");
        var query = new SearchTasksQuery(searchRequest);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.False(result.IsError);
        Assert.Single(result.Value);
        Assert.Equal("Test", result.Value[0].Title);
    }

    [Fact]
    public async void Handle_ReturnsNotFound_WhenNoTasksInDb()
    {
        // Arrange
        _repositoryMock.Setup(r => r.QueryAsync())
            .ReturnsAsync(new List<TaskItem>().AsQueryable());

        var searchRequest = new SearchRequest(Title: "Test",
            Description: "Desc",
            Status: "Open",
            DueDate: DateTime.Now.AddDays(1),
            Priority: "Low",
            UserId: "user-123");
        var query = new SearchTasksQuery(searchRequest);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsError);
        Assert.Contains(result.Errors, e => e.Type == ErrorType.NotFound);
    }

    [Fact]
    public async void Handle_ReturnsNotFound_WhenNoTasksMatchCriteria()
    {
        // Arrange
        var tasks = new List<TaskItem>
        {
            new TaskItem { Id = Guid.NewGuid(), Title = "Test", Description = "Desc", Status = TaskTracker.Domain.Task.Status.New, Priority = TaskTracker.Domain.Task.Priority.High, UserId = "user" }
        }.AsQueryable();

        _repositoryMock.Setup(r => r.QueryAsync())
            .ReturnsAsync(tasks);

        var searchRequest = new SearchRequest(Title: "No Match",
            Description: "Desc",
            Status: "Open",
            DueDate: DateTime.Now.AddDays(1),
            Priority: "Low",
            UserId: "user-123");
        var query = new SearchTasksQuery(searchRequest);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsError);
        Assert.Contains(result.Errors, e => e.Type == ErrorType.NotFound);
    }

    [Fact]
    public async void Handle_ReturnsFailure_WhenExceptionThrown()
    {
        // Arrange
        _repositoryMock.Setup(r => r.QueryAsync())
            .ThrowsAsync(new Exception("DB error"));

        var searchRequest = new SearchRequest(Title: "No Match",
            Description: "Desc",
            Status: "Open",
            DueDate: DateTime.Now.AddDays(1),
            Priority: "Low",
            UserId: "user-123");

        var query = new SearchTasksQuery(searchRequest);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsError);
        Assert.Contains(result.Errors, e => e.Type == ErrorType.Failure);
    }
}
