using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mv.Domain.Entities;

namespace Mv.Infrastructure.Persistence.Configurations.Domain;

public class TicketConfiguration : BaseConfiguration<Ticket> {
  public override void Configure(EntityTypeBuilder<Ticket> builder) {
    base.Configure(builder);

    builder.Property(t => t.Price).HasColumnType("decimal(18,2)");

    builder.OwnsOne(t => t.SeatSnapshot, ss => {
      ss.Property(x => x.SeatId).HasColumnName("SeatId").IsRequired();
      ss.Property(x => x.Row).HasColumnName("SeatRow").IsRequired();
      ss.Property(x => x.Number).HasColumnName("SeatNumber").IsRequired();
    });
  }
}
