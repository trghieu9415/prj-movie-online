using Domain.Base;
using Domain.Enums;
using Domain.ValueObjects;

namespace Domain.Entities;

public class Order : BaseEntity {
  private readonly List<Ticket> _tickets = [];
  private Order() {}
  public Guid? CustomerId { get; private set; }
  public string CustomerName { get; private set; } = null!;
  public Guid ShowtimeId { get; private set; }
  public string AuditoriumName { get; private set; } = null!;


  public OrderStatus Status { get; private set; } = OrderStatus.Pending;
  public decimal TotalPrice { get; private set; }
  public IReadOnlyCollection<Ticket> Tickets => _tickets.AsReadOnly();

  public static Order Create(
    Guid customerId, string customerName,
    Guid showtimeId, string auditoriumName
  ) {
    var order = new Order {
      CustomerId = customerId,
      CustomerName = customerName,
      ShowtimeId = showtimeId,
      AuditoriumName = auditoriumName
    };

    return order;
  }

  public Order SyncTickets(ICollection<SeatSnapshot> seatSnapshots) {
    _tickets.Clear();
    foreach (var seatSnapshot in seatSnapshots) {
      var ticket = Ticket.Create(this, seatSnapshot);
      _tickets.Add(ticket);
      TotalPrice += ticket.Price;
    }

    return this;
  }

  public void MarkAsPaid() {
    Status = OrderStatus.Confirmed;
  }

  public void Cancel() {
    Status = OrderStatus.Canceled;
  }
}
