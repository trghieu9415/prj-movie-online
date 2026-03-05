using Domain.Base;
using Domain.Enums;
using Domain.Events;
using Domain.Exceptions;
using Domain.ValueObjects;

namespace Domain.Entities;

public class Order : BaseEntity {
  private readonly List<Ticket> _tickets = [];
  private Order() {}
  public Guid CustomerId { get; private set; }
  public string CustomerName { get; private set; } = null!;
  public Guid ShowtimeId { get; private set; }
  public string AuditoriumName { get; private set; } = null!;

  public MovieSnapshot Movie { get; private set; } = null!;


  public OrderStatus Status { get; private set; } = OrderStatus.Pending;
  public decimal TotalPrice { get; private set; }
  public IReadOnlyCollection<Ticket> Tickets => _tickets.AsReadOnly();

  public static Order Create(
    Guid customerId, string customerName,
    Guid showtimeId, string auditoriumName,
    MovieSnapshot movie,
    ICollection<SeatSnapshot> seatSnapshots
  ) {
    var order = new Order {
      CustomerId = customerId,
      CustomerName = customerName,
      ShowtimeId = showtimeId,
      AuditoriumName = auditoriumName,
      Movie = movie
    };
    order.SyncTickets(seatSnapshots);
    order.AddDomainEvent(new OrderPlacedEvent(
      order.Id,
      order.CustomerId,
      order.ShowtimeId,
      order.Tickets.Select(x => x.SeatSnapshot.SeatId).ToList()
    ));
    return order;
  }


  public void MarkAsPaid() {
    if (Status != OrderStatus.Pending) {
      throw new DomainException("Chỉ có thể thanh toán đơn khi đơn ở trạng thái Chờ");
    }

    Status = OrderStatus.Confirmed;
    AddDomainEvent(new OrderCompletedEvent(
      Id, CustomerId, ShowtimeId, Tickets.Select(x => x.SeatSnapshot.SeatId).ToList()
    ));
  }

  public void Cancel() {
    if (Status != OrderStatus.Pending) {
      throw new DomainException("Chỉ có thể hủy đơn khi đơn ở trạng thái Chờ");
    }

    Status = OrderStatus.Canceled;
    AddDomainEvent(new OrderCanceledEvent(
      Id, CustomerId, ShowtimeId, Tickets.Select(x => x.SeatSnapshot.SeatId).ToList()
    ));
  }

  public void Refund() {
    if (Status != OrderStatus.Confirmed) {
      throw new DomainException("Chỉ có thể hoàn đơn khi đơn ở trạng thái Đã xác nhận");
    }

    Status = OrderStatus.Refunded;
  }


  private void SyncTickets(ICollection<SeatSnapshot> seatSnapshots) {
    _tickets.Clear();
    TotalPrice = 0;
    foreach (var seatSnapshot in seatSnapshots) {
      var ticket = Ticket.Create(this, seatSnapshot);
      _tickets.Add(ticket);
      TotalPrice += ticket.Price;
    }
  }
}
