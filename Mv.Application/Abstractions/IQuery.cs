using MediatR;

namespace Mv.Application.Abstractions;

public interface IQuery<out TResponse> : IRequest<TResponse>;
