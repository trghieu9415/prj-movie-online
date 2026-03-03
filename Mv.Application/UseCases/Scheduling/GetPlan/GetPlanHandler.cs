using MediatR;
using Mv.Application.Exceptions;
using Mv.Application.Repositories.Read;

namespace Mv.Application.UseCases.Scheduling.GetPlan;

public class GetPlanHandler(IPlanReadRepository planReadRepository)
  : IRequestHandler<GetPlanQuery, GetPlanResult> {
  public async Task<GetPlanResult> Handle(GetPlanQuery request, CancellationToken ct) {
    var planDto =
      await planReadRepository.GetPlanOverviewAsync(request.Id, ct)
      ?? throw new WorkflowException("Kế hoạch không tồn tại", 404);

    return new GetPlanResult(planDto);
  }
}
