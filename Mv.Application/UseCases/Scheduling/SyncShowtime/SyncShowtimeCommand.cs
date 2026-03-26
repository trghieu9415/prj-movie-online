using Mv.Application.Abstractions;
using Mv.Domain.ValueObjects;

namespace Mv.Application.UseCases.Scheduling.SyncShowtime;

public record SyncShowtimeCommand(
  Guid Id,
  List<ShowtimeSnapshot> Showtimes
) : ICommand<bool>;
