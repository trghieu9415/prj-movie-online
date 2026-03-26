namespace Mv.Domain.Base.Event;

public interface IHasDomainEvent {
  IReadOnlyCollection<DomainEvent> DomainEvents { get; }
  public void AddDomainEvent(DomainEvent domainEvent);
  public void ClearEvents();
}
