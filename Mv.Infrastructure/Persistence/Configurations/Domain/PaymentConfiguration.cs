using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mv.Infrastructure.Persistence.Configurations.Domain;

public class PaymentConfiguration : BaseConfiguration<Payment> {
  public override void Configure(EntityTypeBuilder<Payment> builder) {
    base.Configure(builder);

    builder.HasOne<Order>()
      .WithMany()
      .HasForeignKey(p => p.OrderId)
      .IsRequired()
      .OnDelete(DeleteBehavior.Cascade);

    builder.Property(p => p.Amount).HasColumnType("decimal(18,2)");

    // Enums to String
    builder.Property(p => p.Method).HasConversion<string>().HasMaxLength(50);
    builder.Property(p => p.Status).HasConversion<string>().HasMaxLength(50);

    builder.Property(p => p.TransactionId).HasMaxLength(255);
  }
}
