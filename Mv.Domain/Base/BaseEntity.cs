using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Domain.Base.Event;

namespace Domain.Base;

public class BaseEntity : IHasDomainEvent {
  private readonly List<DomainEvent> _domainEvents = [];
  public Guid Id { get; private init; } = Guid.NewGuid();
  public DateTime CreatedAt { get; private init; } = DateTime.UtcNow;
  public DateTime? DeletedAt { get; private set; }
  public bool IsDeleted { get; private set; }
  [JsonIgnore] public uint RowVersion { get; set; }

  [NotMapped] [JsonIgnore] public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();

  public void AddDomainEvent(DomainEvent domainEvent) {
    _domainEvents.Add(domainEvent);
  }

  public void ClearEvents() {
    _domainEvents.Clear();
  }

  public void Delete() {
    IsDeleted = true;
    DeletedAt = DateTime.UtcNow;
  }

  public void Restore() {
    IsDeleted = false;
    DeletedAt = null;
  }
}
