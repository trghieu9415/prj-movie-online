using Domain.ValueObjects;
using Mv.Application.Abstractions;

namespace Mv.Application.UseCases.Scheduling.SyncShowtime;

public record SyncShowtimeCommand(
  Guid Id,
  List<ShowtimeSnapshot> Showtimes
) : ICommand<bool>;
