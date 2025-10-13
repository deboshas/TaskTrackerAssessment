using MediatR;

namespace TaskTracker.Application.CQRS.Abstractions;

public interface  IQuery<out TResponse> : IRequest<TResponse> { }
