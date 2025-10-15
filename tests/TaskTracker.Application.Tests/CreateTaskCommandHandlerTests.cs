using ErrorOr;
using Microsoft.Extensions.Logging;
using Moq;
using System.Threading;
using TaskTracker.Application.Task.Create;
using TaskTracker.Contracts.Common;
using TaskTracker.Contracts.Request.Task;
using TaskTracker.Contracts.Response.Task;
using TaskTracker.Infrastructure.Repositories;
using TaskTracker.Infrastructure.Repositories.Abstractions;

namespace TaskTracker.Application.Tests;

public class CreateTaskCommandHandlerTests
{
    private readonly Mock<ITaskTrackerRepository> _repoMock = new();
    private readonly Mock<ILogger<CreateTaskCommandHandler>> _loggerMock = new();
    private readonly CreateTaskCommandHandler _handler;

    public CreateTaskCommandHandlerTests()
    {
        _handler = new CreateTaskCommandHandler(_repoMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async void Handle_ShouldReturnSuccess_WhenTaskIsCreated()
    {
        // Arrange
        var request = new CreateTaskRequest(
            Title: "Test Task 1",
            Description: "Test Description",
            Priority: Priority.Medium.ToString(),
            DueDate: DateTime.UtcNow.AddDays(1),
            UserId: "user1",
            Status: Status.New.ToString()
        );

        var command = new CreateTaskCommand(request);

        _repoMock.Setup(r => r.Add(It.IsAny<TaskTracker.Domain.Task.TaskItem>()));

        _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), default)).ReturnsAsync(new TaskTracker.Domain.Task.TaskItem
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Description = request.Description,
            Priority = Enum.Parse<TaskTracker.Domain.Task.Priority>(request.Priority),
            DueDate = request.DueDate,
            UserId = request.UserId,
            Status = Enum.Parse<TaskTracker.Domain.Task.Status>(request.Status)
        });

        _repoMock.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()))
                            .ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsError);
        Assert.IsType<TaskResponse>(result.Value);
        _repoMock.Verify(r => r.Add(It.IsAny<TaskTracker.Domain.Task.TaskItem>()), Times.Once);
        _repoMock.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async void Handle_ShouldReturnError_WhenExceptionIsThrown()
    {
        // Arrange
        var request = new CreateTaskRequest(
            Title: "Test Task 2",
            Description: "Test Description",
            Priority: Priority.High.ToString(),
            DueDate: DateTime.UtcNow.AddDays(1),
            UserId: "user2",
            Status: Status.Open.ToString()
        );
        var command = new CreateTaskCommand(request);

        _repoMock.Setup(r => r.Add(It.IsAny<TaskTracker.Domain.Task.TaskItem>()));
        _repoMock.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("DB error"));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsError);
        Assert.Contains("error", result.Errors[0].Description, StringComparison.OrdinalIgnoreCase);
    }
}