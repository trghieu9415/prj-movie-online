using MediatR;
using Mv.Application.Exceptions;
using Mv.Application.Repositories;
using Mv.Domain.Entities;

namespace Mv.Application.UseCases.Scheduling.SyncShowtime;

public class SyncShowtimeHandler(
  IRepository<Listing> listingRepository
) : IRequestHandler<SyncShowtimeCommand, bool> {
  public async Task<bool> Handle(SyncShowtimeCommand request, CancellationToken ct) {
    var listing =
      await listingRepository.GetByIdAsync(request.Id, ct)
      ?? throw new WorkflowException("Không tìm thấy lịch chiếu phim", 404);

    listing.SyncShowtimes(request.Showtimes);
    await listingRepository.UpdateAsync(listing, ct);
    return true;
  }
}
