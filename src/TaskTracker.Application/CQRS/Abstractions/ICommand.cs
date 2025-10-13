using MediatR;

namespace TaskTracker.Application.CQRS.Abstractions;

public  interface  ICommand<out TResponse> : IRequest<TResponse> { }
