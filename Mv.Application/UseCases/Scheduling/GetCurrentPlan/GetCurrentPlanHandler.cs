using MediatR;
using Mv.Application.Exceptions;
using Mv.Application.Ports.Cache;

namespace Mv.Application.UseCases.Scheduling.GetCurrentPlan;

public class GetCurrentPlanHandler(
  IBusinessCache businessCache
) : IRequestHandler<GetCurrentPlanQuery, GetCurrentPlanResult> {
  public async Task<GetCurrentPlanResult> Handle(GetCurrentPlanQuery request, CancellationToken ct) {
    var currentPlan =
      await businessCache.GetCurrentPlanAsync(ct)
      ?? throw new WorkflowException("Chưa có kế hoạch chiếu phim cho thời điểm hiện tại");

    return new GetCurrentPlanResult(currentPlan);
  }
}
