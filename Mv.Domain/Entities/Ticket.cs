using Domain.Base;
using Domain.ValueObjects;

namespace Domain.Entities;

public class Ticket : BaseEntity {
  private Ticket() {}

  public Guid OrderId { get; private set; }
  public Order Order { get; private set; } = null!;
  public SeatSnapshot SeatSnapshot { get; private set; } = null!;
  public decimal Price { get; private set; } = 60000;

  public static Ticket Create(
    Order order, SeatSnapshot seatSnapshot
  ) {
    return new Ticket {
      OrderId = order.Id,
      Order = order,
      SeatSnapshot = seatSnapshot
    };
  }

  public Ticket SetPrice(decimal price) {
    Price = price;
    return this;
  }
}
