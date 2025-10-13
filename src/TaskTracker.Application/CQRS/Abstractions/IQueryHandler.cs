using MediatR;

namespace TaskTracker.Application.CQRS.Abstractions;

/// <summary>
/// Defines a handler for processing queries of type <typeparamref name="TQuery"/> and returning a response of type <typeparamref name="TResponse"/>.
/// </summary>
/// <typeparam name="TQuery">The type of the query being handled. Must implement <see cref="IQuery{TResponse}"/>.</typeparam>
/// <typeparam name="TResponse">The type of the response returned by the query handler.</typeparam>
public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, TResponse>
   where TQuery : IQuery<TResponse>
{ }
