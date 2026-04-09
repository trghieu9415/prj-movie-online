using FluentValidation;
using Mv.Application.Abstractions;

namespace Mv.Application.UseCases.Booking.PlaceOrder;

public record PlaceOrderCommand(
  Guid ShowtimeId,
  List<Guid> SeatIds
) : ICommand<Guid>, ILockable {
  public string LockKey => $"lock:showtime:{ShowtimeId}";
  public TimeSpan WaitTime => TimeSpan.FromSeconds(10);
}

public class PlaceOrderValidator : AbstractValidator<PlaceOrderCommand> {
  public PlaceOrderValidator() {
    RuleFor(x => x.ShowtimeId)
      .NotEmpty().WithMessage("ShowtimeId không hợp lệ.");

    RuleFor(x => x.SeatIds)
      .NotNull().WithMessage("Danh sách ghế không được null.")
      .NotEmpty().WithMessage("Phải chọn ít nhất 1 ghế.");

    RuleForEach(x => x.SeatIds)
      .NotEmpty().WithMessage("SeatId không hợp lệ.");

    RuleFor(x => x.SeatIds)
      .Must(ids => ids.Distinct().Count() == ids.Count)
      .WithMessage("Danh sách ghế không được chứa phần tử trùng.");
  }
}
