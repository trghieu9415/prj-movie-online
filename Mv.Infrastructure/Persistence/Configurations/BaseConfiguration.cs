using Domain.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mv.Infrastructure.Persistence.Configurations;

public abstract class BaseConfiguration<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : BaseEntity {
  public virtual void Configure(EntityTypeBuilder<TEntity> builder) {
    builder.HasKey(e => e.Id);
    builder.Property(e => e.Id).ValueGeneratedNever();
    builder.Property(e => e.IsDeleted).HasDefaultValue(false);
    builder.HasQueryFilter(e => !e.IsDeleted);
  }
}
