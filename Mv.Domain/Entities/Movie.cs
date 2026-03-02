using Domain.Base;

namespace Domain.Entities;

public class Movie : BaseEntity {
  private Movie() { }

  public string Name { get; private set; } = null!;
  public int Duration { get; private set; }
  public string PosterUrl { get; private set; } = null!;

  public static Movie Create(string name, int duration, string posterUrl) {
    var movie = new Movie {
      Name = name,
      Duration = duration,
      PosterUrl = posterUrl
    };
    return movie;
  }

  public Movie Update(string name, int duration, string posterUrl) {
    Name = name;
    Duration = duration;
    PosterUrl = posterUrl;
    return this;
  }
}
