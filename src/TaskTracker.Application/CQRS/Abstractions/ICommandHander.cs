using MediatR;

namespace TaskTracker.Application.CQRS.Abstractions;

public interface ICommandHander<in TCommand, TResponse> : IRequestHandler<TCommand, TResponse>
    where TCommand : ICommand<TResponse>
{ }

