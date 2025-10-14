using FluentValidation;

namespace TaskTracker.Application.Task.Update
{
    public class UpdateTaskCommandValidator : AbstractValidator<UpdateTaskCommand>
    {
        public UpdateTaskCommandValidator()
        {
            RuleFor(x => x.UpdateTaskRequest.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(10).WithMessage("Title must not exceed 10 characters.");

            RuleFor(x => x.UpdateTaskRequest.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(10).WithMessage("Description must not exceed 10 characters.");

            RuleFor(x => x.UpdateTaskRequest.Status)
                .NotEmpty().WithMessage("Status is required.")
                .Must(status => new[] { "Open", "Closed", "New" }
                .Contains(status, StringComparer.OrdinalIgnoreCase))
                .WithMessage("Status must be one of the following values: Open, Closed, or New.");

            RuleFor(x => x.UpdateTaskRequest.Priority)
                .NotEmpty().WithMessage("Priority is required.")
                .Must(priority => new[] { "High", "Medium", "Low" }
                .Contains(priority, StringComparer.OrdinalIgnoreCase))
                .WithMessage("Priority must be one of the following values: High, Medium, or Low.");
        }
    }

}
