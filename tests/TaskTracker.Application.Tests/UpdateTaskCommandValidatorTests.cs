using FluentValidation.TestHelper;
using TaskTracker.Application.Task.Update;
using TaskTracker.Contracts.Request.Task;
using TaskTracker.Domain.Task;
using Xunit;

namespace TaskTracker.Application.Tests;

public class UpdateTaskCommandValidatorTests
{
    private readonly UpdateTaskCommandValidator _validator = new();

    [Fact]
    public void Should_Pass_When_AllFieldsValid()
    {
        var request = new UpdateTaskRequest
        (
            Id : Guid.NewGuid(),
            Title : "ValidTitle",
            Description: "ValidDesc",
            Status: "Open",
            DueDate: DateTime.UtcNow.AddDays(1),
            Priority: "High",
            UserId: "user1"
        );
        var command = new UpdateTaskCommand(request);

        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineData("")]
    
    public void Should_Fail_When_TitleIsEmpty(string title)
    {
        var request = new UpdateTaskRequest
         (
             Id: Guid.NewGuid(),
             Title: title,
             Description: "ValidDesc",
             Status: "Open",
             DueDate: DateTime.UtcNow.AddDays(1),
             Priority: "High",
             UserId: "user1"
         );
        var command = new UpdateTaskCommand(request);

        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.UpdateTaskRequest.Title);
    }

    [Theory]
    [InlineData("ValidTileDesc")]
    public void Should_Fail_When_TitleTooLong(string title)
    {
        var request = new UpdateTaskRequest
         (
             Id: Guid.NewGuid(),
             Title: title,
             Description: "ValidDesc",
             Status: "Open",
             DueDate: DateTime.UtcNow.AddDays(1),
             Priority: "High",
             UserId: "user1"
         );
        var command = new UpdateTaskCommand(request);

        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.UpdateTaskRequest.Title);
    }

    [Theory]
    [InlineData("")]
    public void Should_Fail_When_DescriptionIsEmpty(string description)
    {
        var request = new UpdateTaskRequest
         (
             Id: Guid.NewGuid(),
             Title: "ValidTitle",
             Description: description,
             Status: "Open",
             DueDate: DateTime.UtcNow.AddDays(1),
             Priority: "High",
             UserId: "user1"
         );
        var command = new UpdateTaskCommand(request);

        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.UpdateTaskRequest.Description);
    }

    [Theory]
    [InlineData("validDescriptionLong")]
    public void Should_Fail_When_DescriptionTooLong(string description)
    {
        var request = new UpdateTaskRequest
         (
             Id: Guid.NewGuid(),
             Title: "ValidTitle",
             Description: description,
             Status: "Open",
             DueDate: DateTime.UtcNow.AddDays(1),
             Priority: "High",
             UserId: "user1"
         );
        var command = new UpdateTaskCommand(request);

        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.UpdateTaskRequest.Description);
    }

    [Theory]
    [InlineData("InvalidStatus")]
    public void Should_Fail_When_StatusInvalid(string status)
    {
        var request = new UpdateTaskRequest
        (
            Id: Guid.NewGuid(),
            Title: "ValidTitle",
            Description: "ValidDesc",
            Status: status,
            DueDate: DateTime.UtcNow.AddDays(1),
            Priority: "High",
            UserId: "user1"
        );
        var command = new UpdateTaskCommand(request);

        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.UpdateTaskRequest.Status);
    }

    [Theory]
    [InlineData("InvalidPriority")]
    public void Should_Fail_When_PriorityInvalid(string priority)
    {
        var request = new UpdateTaskRequest
        (
            Id: Guid.NewGuid(),
            Title: "ValidTitle",
            Description: "ValidDesc",
            Status: "Open",
            DueDate: DateTime.UtcNow.AddDays(1),
            Priority: priority,
            UserId: "user1"
        );
        var command = new UpdateTaskCommand(request);

        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.UpdateTaskRequest.Priority);
    }
}
