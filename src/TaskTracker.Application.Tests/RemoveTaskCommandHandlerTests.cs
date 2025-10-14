using ErrorOr;
using Microsoft.Extensions.Logging;
using Moq;
using TaskTracker.Application.Task.Remove;
using TaskTracker.Domain.Task;
using TaskTracker.Infrastructure.Repositories.Abstractions;

namespace TaskTracker.Application.Tests;


public class RemoveTaskCommandHandlerTests
{
    private readonly Mock<ITaskTrackerRepository> _repositoryMock;
    private readonly Mock<ILogger<RemoveTaskCommandHandler>> _loggerMock;
    private readonly RemoveTaskCommandHandler _handler;

    public RemoveTaskCommandHandlerTests()
    {
        _repositoryMock = new Mock<ITaskTrackerRepository>();
        _loggerMock = new Mock<ILogger<RemoveTaskCommandHandler>>();
        _handler = new RemoveTaskCommandHandler(_repositoryMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async void Handle_RemovesTask_WhenTaskExists()
    {
        // Arrange
        var taskId = Guid.NewGuid();
        var taskItem = new TaskItem { Id = taskId, Title = "Test", UserId = "user" };
        _repositoryMock.Setup(r => r.GetByIdAsync(taskId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(taskItem);

        _repositoryMock.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()))
             .Returns(System.Threading.Tasks.Task.FromResult(1));

        // Act
        var result = await _handler.Handle(new RemoveTaskCommand(taskId), CancellationToken.None);

        // Assert
        Assert.False(result.IsError);
        _repositoryMock.Verify(r => r.Delete(taskItem), Times.Once);
        _repositoryMock.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async void Handle_ReturnsNotFound_WhenTaskDoesNotExist()
    {
        // Arrange
        var taskId = Guid.NewGuid();
        _repositoryMock.Setup(r => r.GetByIdAsync(taskId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((TaskItem?)null);

        // Act
        var result = await _handler.Handle(new RemoveTaskCommand(taskId), CancellationToken.None);

        // Assert
        Assert.True(result.IsError);
        Assert.Contains(result.Errors, e => e.Type == ErrorType.NotFound);
    }

    [Fact]
    public async void Handle_ReturnsFailure_WhenExceptionThrown()
    {
        // Arrange
        var taskId = Guid.NewGuid();
        _repositoryMock.Setup(r => r.GetByIdAsync(taskId, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("DB error"));

        // Act
        var result = await _handler.Handle(new RemoveTaskCommand(taskId), CancellationToken.None);

        // Assert
        Assert.True(result.IsError);
        Assert.Contains(result.Errors, e => e.Type == ErrorType.Failure);
    }
}
