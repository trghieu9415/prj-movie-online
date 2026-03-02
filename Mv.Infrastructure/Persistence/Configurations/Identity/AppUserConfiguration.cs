using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mv.Infrastructure.Identity;

namespace Mv.Infrastructure.Persistence.Configurations.Identity;

public class AppUserConfiguration : IEntityTypeConfiguration<AppUser> {
  public void Configure(EntityTypeBuilder<AppUser> builder) {
    builder.HasQueryFilter(e => !e.IsDeleted);

    builder.Property(e => e.FullName).IsRequired().HasMaxLength(100);
    builder.Property(e => e.Url).HasMaxLength(255);
    builder.Property(e => e.IsDeleted).HasDefaultValue(false);

    builder.Property(e => e.Role).HasConversion<string>().HasMaxLength(50);
  }
}
