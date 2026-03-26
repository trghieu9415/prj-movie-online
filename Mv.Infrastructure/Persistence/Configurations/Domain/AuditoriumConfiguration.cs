using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mv.Domain.Entities;

namespace Mv.Infrastructure.Persistence.Configurations.Domain;

public class AuditoriumConfiguration : BaseConfiguration<Auditorium> {
  public override void Configure(EntityTypeBuilder<Auditorium> builder) {
    base.Configure(builder);

    builder.Property(a => a.Name).IsRequired().HasMaxLength(150);

    builder.HasMany(a => a.Seats)
      .WithOne(s => s.Auditorium)
      .HasForeignKey(s => s.AuditoriumId)
      .OnDelete(DeleteBehavior.Cascade);

    builder.Metadata.FindNavigation(nameof(Auditorium.Seats))
      ?.SetPropertyAccessMode(PropertyAccessMode.Field);
  }
}
