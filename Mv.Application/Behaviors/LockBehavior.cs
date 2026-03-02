using MediatR;
using Mv.Application.Abstractions;
using Mv.Application.Exceptions;
using Mv.Application.Ports.Concurrency;

namespace Mv.Application.Behaviors;

public class LockBehavior<TRequest, TResponse>(
  IDistributedLockService lockService
) : IPipelineBehavior<TRequest, TResponse>
  where TRequest : ILockable {
  public async Task<TResponse> Handle(
    TRequest request,
    RequestHandlerDelegate<TResponse> next,
    CancellationToken ct
  ) {
    using var distributedLock = await lockService.AcquireLockAsync(
      request.LockKey,
      request.WaitTime
    );

    if (distributedLock == null) {
      throw new WorkflowException("Hệ thống đang bận xử lý yêu cầu này. Vui lòng thử lại.", 429);
    }

    return await next();
  }
}
