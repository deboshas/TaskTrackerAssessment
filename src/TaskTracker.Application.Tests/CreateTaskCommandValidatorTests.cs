using FluentValidation.TestHelper;
using TaskTracker.Application.Task.Create;
using TaskTracker.Contracts.Request.Task;

namespace TaskTracker.Application.Tests;


public class CreateTaskCommandValidatorTests
{
    private readonly CreateTaskCommandValidator _validator = new();

    [Fact]
    public void Should_Pass_When_AllFieldsValid()
    {
        var request = new CreateTaskRequest
        (
            Title: "ValidTit",
            Description: "Valid Desc",
            Status: "Open",
            DueDate: DateTime.Now.AddDays(1),
            Priority: "High",
            UserId: "user-123"
        );
        var command = new CreateTaskCommand(request);

        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void Should_Fail_When_TitleIsEmpty(string title)
    {
        var request = new CreateTaskRequest
      (
          Title: title,
          Description: "Valid Description",
          Status: "Open",
          DueDate: DateTime.Now.AddDays(1),
          Priority: "High",
          UserId: "user-123"
      );
        var command = new CreateTaskCommand(request);

        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.CreateTaskRequest.Title);
    }

    [Fact]
    public void Should_Fail_When_TitleTooLong()
    {
        var request = new CreateTaskRequest
        (
            Title: "Valid Basic Title",
            Description: "Valid Description",
            Status: "Open",
            DueDate: DateTime.Now.AddDays(1),
            Priority: "High",
            UserId: "user-123"
        );
        var command = new CreateTaskCommand(request);

        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.CreateTaskRequest.Title);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void Should_Fail_When_DescriptionIsEmpty(string description)
    {
        var request = new CreateTaskRequest
       (
           Title: "Valid Title",
           Description: description,
           Status: "Open",
           DueDate: DateTime.Now.AddDays(1),
           Priority: "High",
           UserId: "user-123"
       );
        var command = new CreateTaskCommand(request);

        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.CreateTaskRequest.Description);
    }

    [Fact]
    public void Should_Fail_When_DescriptionTooLong()
    {
        var request = new CreateTaskRequest
       (
           Title: "Valid Title",
           Description: "Valid Description",
           Status: "Open",
           DueDate: DateTime.Now.AddDays(1),
           Priority: "High",
           UserId: "user-123"
       );
        var command = new CreateTaskCommand(request);

        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.CreateTaskRequest.Description);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("InvalidStatus")]
    public void Should_Fail_When_StatusInvalid(string status)
    {
        var request = new CreateTaskRequest
      (
          Title: "Valid Title",
          Description: "Valid Description",
          Status: status,
          DueDate: DateTime.Now.AddDays(1),
          Priority: "High",
          UserId: "user-123"
      );
        var command = new CreateTaskCommand(request);

        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.CreateTaskRequest.Status);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("InvalidPriority")]
    public void Should_Fail_When_PriorityInvalid(string priority)
    {
        var request = new CreateTaskRequest
      (
          Title: "Valid Title",
          Description: "Valid Description",
          Status: "High",
          DueDate: DateTime.Now.AddDays(1),
          Priority: priority,
          UserId: "user-123"
      );
        var command = new CreateTaskCommand(request);

        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.CreateTaskRequest.Priority);
    }
}
