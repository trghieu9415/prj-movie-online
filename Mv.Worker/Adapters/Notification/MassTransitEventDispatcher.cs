using Domain.Base.Event;
using MassTransit;
using Mv.Application.Ports.Notification;
using Mv.Infrastructure.Persistence;

namespace Mv.Worker.Adapters.Notification;

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

    domainEntities.ForEach(x => x.ClearEvents());

    await Task.WhenAll(domainEvents.Select(evt => publishEndpoint.Publish((object)evt, ct)));
  }
}
