using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mv.Domain.Entities;

namespace Mv.Infrastructure.Persistence.Configurations.Domain;

public class OrderConfiguration : BaseConfiguration<Order> {
  public override void Configure(EntityTypeBuilder<Order> builder) {
    base.Configure(builder);

    builder.Property(o => o.CustomerName).IsRequired().HasMaxLength(255);
    builder.Property(o => o.CustomerEmail).IsRequired().HasMaxLength(255);
    builder.Property(o => o.AuditoriumName).IsRequired().HasMaxLength(150);

    // Store Enum as string
    builder.Property(o => o.Status).HasConversion<string>().HasMaxLength(50);

    builder.Property(o => o.TotalPrice).HasPrecision(18, 2);

    builder.HasMany(o => o.Tickets)
      .WithOne(t => t.Order)
      .HasForeignKey(t => t.OrderId)
      .OnDelete(DeleteBehavior.Cascade);

    builder.OwnsOne(o => o.Movie, snapshot => {
      snapshot.Property(s => s.Id).HasColumnName("MovieId");
      snapshot.Property(s => s.Name).HasColumnName("MovieName");
      snapshot.Property(s => s.PosterUrl).HasColumnName("MoviePosterUrl");
    });

    builder.Metadata.FindNavigation(nameof(Order.Tickets))
      ?.SetPropertyAccessMode(PropertyAccessMode.Field);
  }
}
