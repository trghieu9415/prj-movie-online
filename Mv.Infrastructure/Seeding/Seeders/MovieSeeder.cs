using Bogus;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Mv.Infrastructure.Persistence;

namespace Mv.Infrastructure.Seeding.Seeders;

public class MovieSeeder(AppDbContext context) : ISeeder {
  public int Order => 2;

  public async Task SeedAsync() {
    if (await context.Set<Movie>().AnyAsync()) {
      return;
    }

    Console.WriteLine("[+] Seeding Movies...");
    var faker = new Faker("vi");
    var movies = new List<Movie>();

    for (var i = 0; i < 20; i++) {
      var name = faker.Lorem.Sentence(new Random().Next(2, 6)).TrimEnd('.');
      var duration = faker.Random.Int(90, 180);
      var imageUrl = $"https://api.dicebear.com/9.x/shapes/svg?seed={Uri.EscapeDataString(name)}";
      var movie = Movie.Create(name, duration, imageUrl);
      movies.Add(movie);
    }

    context.Set<Movie>().AddRange(movies);
    await context.SaveChangesAsync();
  }
}
