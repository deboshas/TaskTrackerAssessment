using MediatR;

namespace TaskTracker.Application.CQRS.Abstractions;

/// <summary>
/// Represents a command in the CQRS pattern that returns a response of type <typeparamref name="TResponse"/>.
/// </summary>
/// <typeparam name="TResponse">The type of the response returned by the command.</typeparam>
public interface ICommand<out TResponse> : IRequest<TResponse>
{
    // This interface inherits from MediatR's IRequest interface, 
    // allowing it to be used as a marker for commands in the application.
}
