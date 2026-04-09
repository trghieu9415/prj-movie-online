using FluentValidation;
using Mv.Application.Abstractions;
using Mv.Domain.ValueObjects;

namespace Mv.Application.UseCases.Scheduling.SyncShowtime;

public record SyncShowtimeCommand(
  Guid Id,
  List<ShowtimeSnapshot> Showtimes
) : ICommand<bool>;

public class SyncShowtimeValidator : AbstractValidator<SyncShowtimeCommand> {
  public SyncShowtimeValidator() {
    RuleFor(x => x.Id)
      .NotEmpty().WithMessage("Id không hợp lệ.");

    RuleFor(x => x.Showtimes)
      .NotNull().WithMessage("Danh sách Showtimes không được null.")
      .Must(x => x.Count > 0).WithMessage("Danh sách Showtimes phải có ít nhất 1 phần tử.");
  }
}
