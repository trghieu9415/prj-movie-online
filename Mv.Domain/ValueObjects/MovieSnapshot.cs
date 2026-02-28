namespace Domain.ValueObjects;

public record MovieSnapshot(Guid MovieId, string Title, int Duration, string PosterUrl);
