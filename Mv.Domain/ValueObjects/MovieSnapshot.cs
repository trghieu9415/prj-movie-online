namespace Domain.ValueObjects;

public record MovieSnapshot {
  public Guid Id { get; init; }
  public string Name { get; init; } = string.Empty;
  public string PosterUrl { get; init; } = string.Empty;
}
