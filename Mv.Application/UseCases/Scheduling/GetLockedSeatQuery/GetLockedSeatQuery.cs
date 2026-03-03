using Mv.Application.Abstractions;

namespace Mv.Application.UseCases.Scheduling.GetLockedSeatQuery;

public record GetLockedSeatQuery(Guid Id) : IQuery<GetLockedSeatResult>;

public record GetLockedSeatResult(List<Guid> SeatIds);
