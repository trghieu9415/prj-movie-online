using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mv.Infrastructure.Persistence.Configurations.Domain;

public class PlanConfiguration : BaseConfiguration<Plan> {
  public override void Configure(EntityTypeBuilder<Plan> builder) {
    base.Configure(builder);

    builder.Property(p => p.Name).IsRequired().HasMaxLength(255);

    builder.HasMany(p => p.Listings)
      .WithOne()
      .OnDelete(DeleteBehavior.Cascade);

    builder.Metadata.FindNavigation(nameof(Plan.Listings))
      ?.SetPropertyAccessMode(PropertyAccessMode.Field);
  }
}
