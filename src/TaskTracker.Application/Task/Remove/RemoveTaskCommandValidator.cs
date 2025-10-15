using FluentValidation;

namespace TaskTracker.Application.Task.Remove;
public class RemoveTaskCommandValidator : AbstractValidator<RemoveTaskCommand>
{
    public RemoveTaskCommandValidator()
    {
        RuleFor(x => x.TaskId)
            .NotEmpty().WithMessage("TaskId is required.")
            .Must(taskId => Guid.TryParse(taskId.ToString(), out _)).WithMessage("TaskId must be a valid GUID.");
    }
}
