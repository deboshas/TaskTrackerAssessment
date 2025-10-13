using MediatR;

namespace TaskTracker.Application.CQRS.Abstractions;

public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, TResponse>
    where TQuery : IQuery<TResponse>
{ }
