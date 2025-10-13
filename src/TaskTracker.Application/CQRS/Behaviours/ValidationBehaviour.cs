using FluentValidation;
using MediatR;


namespace TaskTracker.Application.CQRS.Behaviours;

public sealed class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
   where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    /// <summary>  
    /// Initializes a new instance of the <see cref="ValidationBehaviour{TRequest, TResponse}"/> class.  
    /// </summary>  
    /// <param name="validators">A collection of validators for the request type.</param>  
    public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    /// <summary>  
    /// Handles the validation of the request before passing it to the next pipeline behavior or handler.  
    /// </summary>  
    /// <param name="request">The incoming request to validate.</param>  
    /// <param name="next">The next delegate in the pipeline.</param>  
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>  
    /// <returns>The response from the next pipeline behavior or handler.</returns>  
    /// <exception cref="ValidationException">Thrown if validation fails for the request.</exception>  
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (_validators.Any())
        {
            var context = new ValidationContext<TRequest>(request);

            var validationResults = await System.Threading.Tasks.Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));
            var failures = validationResults.SelectMany(r => r.Errors).Where(f => f != null).ToList();

            if (failures.Count != 0)
            {
                throw new ValidationException(failures);
            }
        }

        return await next();
    }
}
