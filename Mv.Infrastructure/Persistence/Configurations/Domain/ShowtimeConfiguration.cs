using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mv.Infrastructure.Persistence.Configurations.Domain;

public class ShowtimeConfiguration : BaseConfiguration<Showtime> {
  public override void Configure(EntityTypeBuilder<Showtime> builder) {
    base.Configure(builder);

    builder.HasOne<Auditorium>()
      .WithMany()
      .HasForeignKey(s => s.AuditoriumId)
      .IsRequired()
      .OnDelete(DeleteBehavior.Cascade);
  }
}
