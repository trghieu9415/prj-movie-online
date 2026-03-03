using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Mv.Infrastructure.Persistence;

namespace Mv.Infrastructure.Seeding.Seeders;

public class ListingSeeder(AppDbContext context) : ISeeder {
  public int Order => 4;

  public async Task SeedAsync() {
    var hasShowtimes = await context.Set<Listing>().AnyAsync(l => l.Showtimes.Any());
    if (hasShowtimes) {
      return;
    }

    var plans = await context.Set<Plan>()
      .Include(p => p.Listings)
      .ThenInclude(l => l.Showtimes)
      .ToListAsync();

    if (plans.Count == 0 || plans.All(p => p.Listings.Count == 0)) {
      Console.WriteLine("    -> No Listings found in Plans to seed Showtimes.");
      return;
    }

    Console.WriteLine("[+] Seeding Showtimes for Listings...");

    var auditoriums = await context.Set<Auditorium>().ToListAsync();
    if (auditoriums.Count == 0) {
      Console.WriteLine("    -> No Auditoriums found. Generating default Auditoriums...");
      for (var i = 1; i <= 5; i++) {
        var aud = Auditorium.Create($"Phòng chiếu {i}");
        aud.GenerateSeatMatrix(8, 10);
        auditoriums.Add(aud);
      }

      context.Set<Auditorium>().AddRange(auditoriums);
      await context.SaveChangesAsync();
    }

    var random = new Random();

    foreach (var plan in plans) {
      foreach (var listing in plan.Listings) {
        var snapshots = new List<ShowtimeSnapshot>();

        for (var date = plan.StartDate; date <= plan.EndDate; date = date.AddDays(1)) {
          var numShowtimes = random.Next(2, 6);

          for (var i = 0; i < numShowtimes; i++) {
            var aud = auditoriums[random.Next(auditoriums.Count)];

            var startHour = random.Next(9, 21);
            var startMinute = random.Next(0, 2) * 30;
            var startAt = new TimeSpan(startHour, startMinute, 0);

            var endAt = startAt.Add(TimeSpan.FromHours(2).Add(TimeSpan.FromMinutes(15)));
            snapshots.Add(new ShowtimeSnapshot(null, aud.Id, date, startAt, endAt));
          }
        }

        listing.SyncShowtimes(snapshots);
      }
    }

    await context.SaveChangesAsync();
  }
}
