using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Mv.Infrastructure.Persistence;

namespace Mv.Infrastructure.Seeding.Seeders;

public class PlanSeeder(AppDbContext context) : ISeeder {
  public int Order => 3;

  public async Task SeedAsync() {
    if (await context.Set<Plan>().AnyAsync()) {
      return;
    }

    Console.WriteLine("[+] Seeding Plans...");
    var movies = await context.Set<Movie>().ToListAsync();
    if (movies.Count == 0) {
      Console.WriteLine("    -> No Movies found to create Plans. Skipping.");
      return;
    }

    var random = new Random();
    var plans = new List<Plan>();
    var currentYear = DateTime.Now.Year;
    var currentMonth = DateTime.Now.Month;

    for (var w = 1; w <= 4; w++) {
      var plan = Plan.Create(null, currentYear, currentMonth, w);

      var takeCount = random.Next(5, 11);
      var selectedMovieIds = movies.OrderBy(m => random.Next()).Take(takeCount).Select(m => m.Id).ToList();
      plan.SyncListings(selectedMovieIds);

      plans.Add(plan);
    }

    context.Set<Plan>().AddRange(plans);
    await context.SaveChangesAsync();
  }
}
