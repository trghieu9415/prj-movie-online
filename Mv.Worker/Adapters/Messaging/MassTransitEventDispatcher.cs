using Domain.Base.Event;
using MassTransit;
using Mv.Application.Ports.Messaging;
using Mv.Infrastructure.Persistence;

namespace Mv.Worker.Adapters.Messaging;

public class MassTransitEventDispatcher(
  AppDbContext dbContext,
  IPublishEndpoint publishEndpoint
) : IEventDispatcher {
  public async Task DispatchEventsAsync(CancellationToken ct = default) {
    var domainEntities = dbContext.ChangeTracker
      .Entries<IHasDomainEvent>()
      .Where(x => x.Entity.DomainEvents.Count != 0)
      .Select(x => x.Entity)
      .ToList();

    if (domainEntities.Count == 0) {
      return;
    }

    var domainEvents = domainEntities
      .SelectMany(x => x.DomainEvents)
      .ToList();

    foreach (var domainEvent in domainEvents) {
      await publishEndpoint.Publish((object)domainEvent, ct);
    }

    domainEntities.ForEach(x => x.ClearEvents());
  }
}
