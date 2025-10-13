using FluentValidation;

namespace TaskTracker.Application.Task.Create
{
    public class CreateTaskCommandValidator : AbstractValidator<CreateTaskCommand>
    {
        public CreateTaskCommandValidator()
        {
            RuleFor(x => x.CreateTaskRequest.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(100).WithMessage("Title must not exceed 100 characters.");

            RuleFor(x => x.CreateTaskRequest.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(100).WithMessage("Description must not exceed 100 characters.");




        }
    }

}
