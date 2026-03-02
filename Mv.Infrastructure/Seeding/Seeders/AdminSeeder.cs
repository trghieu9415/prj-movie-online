using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Mv.Application.Models;
using Mv.Infrastructure.Persistence.Identity;

namespace Mv.Infrastructure.Seeding.Seeders;

public class AdminSeeder(UserManager<AppUser> userManager) : ISeeder {
  public int Order => 1;

  public async Task SeedAsync() {
    if (await userManager.Users.AnyAsync(u => u.Role == UserRole.Admin)) {
      return;
    }

    var admin = new AppUser {
      Id = Guid.NewGuid(),
      UserName = "admin@cinema.com",
      Email = "admin@cinema.com",
      FullName = "System Admin",
      Role = UserRole.Admin,
      EmailConfirmed = true
    };
    await userManager.CreateAsync(admin, "222aaa,,,");
  }
}
