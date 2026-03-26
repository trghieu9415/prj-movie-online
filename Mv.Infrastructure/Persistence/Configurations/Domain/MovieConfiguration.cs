using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mv.Domain.Entities;

namespace Mv.Infrastructure.Persistence.Configurations.Domain;

public class MovieConfiguration : BaseConfiguration<Movie> {
  public override void Configure(EntityTypeBuilder<Movie> builder) {
    base.Configure(builder);

    builder.Property(m => m.Name).IsRequired().HasMaxLength(255);
    builder.Property(m => m.PosterUrl).IsRequired().HasMaxLength(1000);
  }
}
