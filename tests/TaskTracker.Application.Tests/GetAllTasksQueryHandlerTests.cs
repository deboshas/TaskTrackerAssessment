using ErrorOr;
using Microsoft.Extensions.Logging;
using Moq;
using TaskTracker.Application.Task.GetAll;
using TaskTracker.Domain.Task;
using TaskTracker.Infrastructure.Repositories.Abstractions;


namespace TaskTracker.Application.Tests;

public class GetAllTasksQueryHandlerTests
{
    private readonly Mock<ITaskTrackerRepository> _repositoryMock;
    private readonly Mock<ILogger<GetAllTasksQueryHandler>> _loggerMock;
    private readonly GetAllTasksQueryHandler _handler;

    public GetAllTasksQueryHandlerTests()
    {
        _repositoryMock = new Mock<ITaskTrackerRepository>();
        _loggerMock = new Mock<ILogger<GetAllTasksQueryHandler>>();
        _handler = new GetAllTasksQueryHandler(_repositoryMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async void Handle_ReturnsTaskResponses_WhenTasksExist()
    {
        // Arrange
        var tasks = new List<TaskItem>
        {
            new TaskItem
            {
                Id = Guid.NewGuid(),
                Title = "Test Task",
                Description = "Description",
                Status = Status.New,
                DueDate = DateTime.UtcNow,
                Priority = Priority.High,
                UserId = "user1"
            }
        }.AsQueryable();

        _repositoryMock.Setup(r => r.QueryAsync())
            .ReturnsAsync(tasks);

        // Act
        var result = await _handler.Handle(new GetAllTasksQuery(), CancellationToken.None);

        // Assert
        Assert.False(result.IsError);
        var responses = result.Value;
        Assert.Single(responses);
        Assert.Equal("Test Task", responses[0].Title);
    }

    [Fact]
    public async void Handle_ReturnsNotFoundError_WhenNoTasksExist()
    {
        // Arrange
        _repositoryMock.Setup(r => r.QueryAsync())
            .ReturnsAsync(new List<TaskItem>().AsQueryable());

        // Act
        var result = await _handler.Handle(new GetAllTasksQuery(), CancellationToken.None);

        // Assert
        Assert.True(result.IsError);
        Assert.Contains(result.Errors, e => e.Type == ErrorType.NotFound);
    }

    [Fact]
    public async void Handle_ReturnsFailureError_WhenExceptionThrown()
    {
        // Arrange
        _repositoryMock.Setup(r => r.QueryAsync())
            .ThrowsAsync(new Exception("DB error"));

        // Act
        var result = await _handler.Handle(new GetAllTasksQuery(), CancellationToken.None);

        // Assert
        Assert.True(result.IsError);
        Assert.Contains(result.Errors, e => e.Type == ErrorType.Failure);
    }
}
