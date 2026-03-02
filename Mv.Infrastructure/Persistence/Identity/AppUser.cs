using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Mv.Application.Models;

namespace Mv.Infrastructure.Persistence.Identity;

public class AppUser : IdentityUser<Guid> {
  [MaxLength(100)] public string FullName { get; set; } = null!;
  [MaxLength(255)] public string? Url { get; set; }
  public DateTime CreatedAt { get; private init; } = DateTime.UtcNow;
  public DateTime? DeletedAt { get; private set; }
  public bool IsDeleted { get; private set; }

  public UserRole Role { get; init; } = UserRole.Customer;

  public void Update(string fullName, string? url) {
    FullName = fullName;
    Url = url;
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
