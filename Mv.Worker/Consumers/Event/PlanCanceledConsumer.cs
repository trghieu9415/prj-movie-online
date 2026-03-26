using MassTransit;
using Mv.Application.Constants;
using Mv.Application.DTOs;
using Mv.Domain.Events;
using Mv.Infrastructure.Services.Abstractions;

namespace Mv.Worker.Consumers.Event;

public class PlanCanceledConsumer(
  ICacheService cacheService
) : IConsumer<PlanCanceledEvent> {
  public async Task Consume(ConsumeContext<PlanCanceledEvent> context) {
    var cachedPlan = await cacheService.GetAsync<PlanDto>(
      CacheTags.CurrentPlan,
      context.CancellationToken
    );
    if (cachedPlan != null) {
      await cacheService.RemoveAsync(CacheTags.CurrentPlan, context.CancellationToken);
    }
  }
}
