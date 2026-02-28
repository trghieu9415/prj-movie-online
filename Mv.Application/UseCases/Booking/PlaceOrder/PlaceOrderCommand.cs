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
    RuleFor(x => x.ShowtimeId).NotEmpty();
    RuleFor(x => x.SeatIds).NotEmpty().WithMessage("Phải chọn ít nhất một ghế");
  }
}
