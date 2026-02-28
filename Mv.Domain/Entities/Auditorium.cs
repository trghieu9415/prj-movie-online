using Domain.Base;
using Domain.Exceptions;

namespace Domain.Entities;

public class Auditorium : BaseEntity {
  private readonly List<Seat> _seats = [];
  private Auditorium() {}
  public string Name { get; private set; } = null!;
  public IReadOnlyCollection<Seat> Seats => _seats.AsReadOnly();


  public static Auditorium Create(string name) {
    return new Auditorium {
      Name = name
    };
  }

  public Auditorium Update(string name) {
    Name = name;
    return this;
  }

  public Auditorium GenerateSeatMatrix(
    int rowCount,
    int seatsPerRow
  ) {
    if (_seats.Count != 0) {
      throw new DomainException("Không thể tạo sơ đồ ghế vì rạp đã có ghế");
    }

    for (var r = 0; r < rowCount; r++) {
      var rowChar = (char)('A' + r);

      for (var n = 1; n <= seatsPerRow; n++) {
        AddSeat(rowChar, n);
      }
    }

    return this;
  }

  private void AddSeat(char row, int number) {
    _seats.Add(Seat.Create(this, row, number));
  }
}
