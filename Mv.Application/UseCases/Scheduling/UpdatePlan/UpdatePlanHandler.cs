using MediatR;
using Mv.Application.Exceptions;
using Mv.Application.Repositories;
using Mv.Domain.Entities;

namespace Mv.Application.UseCases.Scheduling.UpdatePlan;

public class UpdatePlanHandler(IRepository<Plan> planRepository)
  : IRequestHandler<UpdatePlanCommand, bool> {
  public async Task<bool> Handle(UpdatePlanCommand request, CancellationToken ct) {
    var plan =
      await planRepository.GetByIdAsync(request.Id, ct)
      ?? throw new WorkflowException("Không tìm thấy kế hoạch", 404);

    plan.Update(request.Name, request.StartDate, request.EndDate);
    await planRepository.UpdateAsync(plan, ct);
    return true;
  }
}
