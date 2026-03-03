using MediatR;
using Mv.Application.Repositories.Read;

namespace Mv.Application.UseCases.Scheduling.GetLockedSeatQuery;

public class GetLockedSeatHandler(
  IShowtimeReadRepository showtimeReadRepository
) : IRequestHandler<GetLockedSeatQuery, GetLockedSeatResult> {
  public async Task<GetLockedSeatResult> Handle(GetLockedSeatQuery request, CancellationToken ct) {
    return new GetLockedSeatResult(await showtimeReadRepository.GetLockedSeatIdsAsync(request.Id, ct));
  }
}
