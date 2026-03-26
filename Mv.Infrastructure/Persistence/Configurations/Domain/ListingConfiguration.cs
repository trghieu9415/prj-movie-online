using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mv.Domain.Entities;

namespace Mv.Infrastructure.Persistence.Configurations.Domain;

public class ListingConfiguration : BaseConfiguration<Listing> {
  public override void Configure(EntityTypeBuilder<Listing> builder) {
    base.Configure(builder);

    builder.HasOne<Movie>()
      .WithMany()
      .HasForeignKey(l => l.MovieId)
      .IsRequired();

    builder.HasMany(l => l.Showtimes)
      .WithOne()
      .OnDelete(DeleteBehavior.Cascade);

    builder.Metadata.FindNavigation(nameof(Listing.Showtimes))
      ?.SetPropertyAccessMode(PropertyAccessMode.Field);
  }
}
