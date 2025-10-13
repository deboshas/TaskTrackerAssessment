using MediatR;

namespace TaskTracker.Application.CQRS.Abstractions;

/// <summary>
/// Defines a handler for processing commands of type <typeparamref name="TCommand"/> and returning a response of type <typeparamref name="TResponse"/>.
/// </summary>
/// <typeparam name="TCommand">The type of the command to handle.</typeparam>
/// <typeparam name="TResponse">The type of the response returned after handling the command.</typeparam>
public interface ICommandHander<in TCommand, TResponse> : IRequestHandler<TCommand, TResponse>
   where TCommand : ICommand<TResponse>
{ }

