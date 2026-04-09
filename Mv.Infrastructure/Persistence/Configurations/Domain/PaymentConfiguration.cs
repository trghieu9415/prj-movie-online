using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mv.Domain.Entities;

namespace Mv.Infrastructure.Persistence.Configurations.Domain;

public class PaymentConfiguration : BaseConfiguration<Payment> {
  public override void Configure(EntityTypeBuilder<Payment> builder) {
    base.Configure(builder);

    builder.HasOne<Order>()
      .WithMany()
      .HasForeignKey(p => p.OrderId)
      .IsRequired()
      .OnDelete(DeleteBehavior.Cascade);

    builder.Property(p => p.Amount).HasPrecision(18, 2);
    builder.Property(p => p.PaymentUrl).HasColumnType("text");
    ;


    // Enums to String
    builder.Property(p => p.Method).HasConversion<string>().HasMaxLength(50);
    builder.Property(p => p.Status).HasConversion<string>().HasMaxLength(50);

    builder.Property(p => p.TransactionId).HasMaxLength(255);
  }
}
