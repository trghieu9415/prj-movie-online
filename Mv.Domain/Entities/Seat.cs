using Domain.Base;
using Domain.ValueObjects;

namespace Domain.Entities;

public class Seat : BaseEntity {
  private Seat() {}

  public Guid AuditoriumId { get; protected set; }
  public Auditorium Auditorium { get; protected set; } = null!;

  public char Row { get; private set; }
  public int Number { get; private set; }

  public static Seat Create(
    Auditorium auditorium,
    char row, int number
  ) {
    var seat = new Seat {
      Auditorium = auditorium,
      AuditoriumId = auditorium.Id,
      Row = row,
      Number = number
    };
    return seat;
  }

  public SeatSnapshot ToSnapshot() {
    return new SeatSnapshot(Id, Row, Number);
  }
}
