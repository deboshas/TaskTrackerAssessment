using FluentValidation;

namespace TaskTracker.Application.Task.Create
{
    public class UpdateTaskCommandValidator : AbstractValidator<CreateTaskCommand>
    {
        public UpdateTaskCommandValidator()
        {
            RuleFor(x => x.CreateTaskRequest.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(100).WithMessage("Title must not exceed 100 characters.");

            RuleFor(x => x.CreateTaskRequest.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(100).WithMessage("Description must not exceed 100 characters.");

            RuleFor(x => x.CreateTaskRequest.Status)
                                      .NotEmpty().WithMessage("Status is required.")
                                      .Must(status => new[] { "Open", "Closed", "New" }
                                      .Contains(status, StringComparer.OrdinalIgnoreCase))
                                      .WithMessage("Status must be one of the following values: Open, Closed, or New.");

            RuleFor(x => x.CreateTaskRequest.Priority)
                           .NotEmpty().WithMessage("Priority is required.")
                           .Must(priority => new[] { "High", "Medium", "Low" }
                           .Contains(priority, StringComparer.OrdinalIgnoreCase))
                           .WithMessage("Priority must be one of the following values: High, Medium, or Low.");



        }
    }

}
