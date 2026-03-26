using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mv.Domain.Entities;

namespace Mv.Infrastructure.Persistence.Configurations.Domain;

public class SeatConfiguration : BaseConfiguration<Seat> {
  public override void Configure(EntityTypeBuilder<Seat> builder) {
    base.Configure(builder);

    builder.Property(s => s.Row).IsRequired();
    builder.Property(s => s.Number).IsRequired();

    builder.HasIndex(s => new { s.AuditoriumId, s.Row, s.Number })
      .IsUnique()
      .HasFilter("\"IsDeleted\" = false");
  }
}
