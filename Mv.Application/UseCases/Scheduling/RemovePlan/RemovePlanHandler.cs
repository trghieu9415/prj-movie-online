using Domain.Entities;
using MediatR;
using Mv.Application.Repositories;

namespace Mv.Application.UseCases.Scheduling.RemovePlan;

public class RemovePlanHandler(IRepository<Plan> planRepository)
  : IRequestHandler<RemovePlanCommand, bool> {
  public async Task<bool> Handle(RemovePlanCommand request, CancellationToken ct) {
    await planRepository.DeleteAsync(request.Id, true, ct);
    return true;
  }
}
