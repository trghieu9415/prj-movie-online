using Microsoft.EntityFrameworkCore;
using Mv.Domain.Entities;
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

    var today = DateOnly.FromDateTime(DateTime.UtcNow);

    var diff = (7 + (today.DayOfWeek - DayOfWeek.Monday)) % 7;
    var startOfFirstWeek = today.AddDays(-1 * diff);

    for (var i = 0; i < 4; i++) {
      var startDate = startOfFirstWeek.AddDays(i * 7);
      var endDate = startDate.AddDays(6);

      var planName = $"Lịch chiếu Tuần {i + 1} ({startDate:dd/MM} - {endDate:dd/MM})";
      var plan = Plan.Create(planName, startDate, endDate);

      var takeCount = random.Next(5, 11);
      var selectedMovieIds = movies
        .OrderBy(_ => random.Next()).Take(takeCount).Select(m => m.Id).ToList();
      plan.SyncListings(selectedMovieIds);
      plans.Add(plan);
    }

    context.Set<Plan>().AddRange(plans);
    await context.SaveChangesAsync();
  }
}
