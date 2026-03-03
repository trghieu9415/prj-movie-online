using Domain.Entities;
using MediatR;
using Mv.Application.Repositories;

namespace Mv.Application.UseCases.Scheduling.CreatePlan;

public class CreatePlanHandler(IRepository<Plan> planRepository)
  : IRequestHandler<CreatePlanCommand, Guid> {
  public async Task<Guid> Handle(CreatePlanCommand request, CancellationToken ct) {
    var plan = Plan.Create(request.Name, request.StartDate, request.EndDate);
    return await planRepository.CreateAsync(plan, ct);
  }
}
