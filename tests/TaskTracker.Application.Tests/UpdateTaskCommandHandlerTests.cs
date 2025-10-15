using ErrorOr;
using Microsoft.Extensions.Logging;
using Moq;
using TaskTracker.Application.Task.Create;
using TaskTracker.Application.Task.Update;
using TaskTracker.Contracts.Request.Task;
using TaskTracker.Domain.Task;
using TaskTracker.Infrastructure.Repositories.Abstractions;

namespace TaskTracker.Application.Tests;

public class UpdateTaskCommandHandlerTests
{
    private readonly Mock<ITaskTrackerRepository> _repositoryMock;
    private readonly Mock<ILogger<CreateTaskCommandHandler>> _loggerMock;
    private readonly UpdateTaskCommandHandler _handler;

    public UpdateTaskCommandHandlerTests()
    {
        _repositoryMock = new Mock<ITaskTrackerRepository>();
        _loggerMock = new Mock<ILogger<CreateTaskCommandHandler>>();
        _handler = new UpdateTaskCommandHandler(_repositoryMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async void Handle_UpdatesTask_WhenTaskExists()
    {
        // Arrange
        var taskId = Guid.NewGuid();
        var taskItem = new TaskItem { Id = taskId, Title = "Test", UserId = "user" ,Status=TaskTracker.Domain.Task.Status.Open,Priority=TaskTracker.Domain.Task.Priority.Low};
        var updateRequest = new UpdateTaskRequest(Id: taskId, Title: "Updated", UserId: "user", Status:"Open",Priority: "Low");
        var updateCommand = new UpdateTaskCommand(updateRequest);

        _repositoryMock.Setup(r => r.GetByIdAsync(taskId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(taskItem);

        // MapUpdatedTask should return a TaskItem (simulate mapping)
        // If you use an extension, you may need to mock it or use a real implementation
        // For simplicity, let's assume it returns a new TaskItem
        _repositoryMock.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .Returns(System.Threading.Tasks.Task.FromResult(1));

        // Act
        var result = await _handler.Handle(updateCommand, CancellationToken.None);

        // Assert
        Assert.False(result.IsError);
        _repositoryMock.Verify(r => r.Update(It.IsAny<TaskItem>()), Times.Once);
        _repositoryMock.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async void Handle_ReturnsNotFound_WhenTaskDoesNotExist()
    {
        // Arrange
        var taskId = Guid.NewGuid();
        var updateRequest = new UpdateTaskRequest(Id: taskId, Title: "Updated", UserId: "user");
        var updateCommand = new UpdateTaskCommand(updateRequest);

        _repositoryMock.Setup(r => r.GetByIdAsync(taskId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((TaskItem?)null);

        // Act
        var result = await _handler.Handle(updateCommand, CancellationToken.None);

        // Assert
        Assert.True(result.IsError);
        Assert.Contains(result.Errors, e => e.Type == ErrorType.NotFound);
    }

    [Fact]
    public async void Handle_ReturnsFailure_WhenExceptionThrown()
    {
        // Arrange
        var taskId = Guid.NewGuid();
        var updateRequest = new UpdateTaskRequest(Id: taskId, Title: "Updated", UserId: "user");
        var updateCommand = new UpdateTaskCommand(updateRequest);

        _repositoryMock.Setup(r => r.GetByIdAsync(taskId, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("DB error"));

        // Act
        var result = await _handler.Handle(updateCommand, CancellationToken.None);

        // Assert
        Assert.True(result.IsError);
        Assert.Contains(result.Errors, e => e.Type == ErrorType.Failure);
    }
}
