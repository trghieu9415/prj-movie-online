using MediatR;
using Mv.Application.Repositories;
using Mv.Domain.Entities;

namespace Mv.Application.UseCases.Scheduling.RemovePlan;

public class RemovePlanHandler(IRepository<Plan> planRepository)
  : IRequestHandler<RemovePlanCommand, bool> {
  public async Task<bool> Handle(RemovePlanCommand request, CancellationToken ct) {
    await planRepository.DeleteAsync(request.Id, true, ct);
    return true;
  }
}
