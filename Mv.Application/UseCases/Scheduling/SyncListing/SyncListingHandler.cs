using Domain.Entities;
using MediatR;
using Mv.Application.Exceptions;
using Mv.Application.Repositories;

namespace Mv.Application.UseCases.Scheduling.SyncListing;

public class SyncListingHandler(
  IRepository<Plan> planRepository,
  IRepository<Movie> movieRepository
) : IRequestHandler<SyncListingCommand, bool> {
  public async Task<bool> Handle(SyncListingCommand request, CancellationToken ct) {
    var plan =
      await planRepository.GetByIdAsync(request.Id, ct)
      ?? throw new WorkflowException("Không tìm thấy kế hoạch", 404);

    var missingMovieIds = await movieRepository.GetMissingIdsAsync(request.MovieIds, ct);
    if (missingMovieIds.Count > 0) {
      throw new WorkflowException(
        $"Phim với Id {string.Join(", ", missingMovieIds)} không tồn tại",
        404
      );
    }

    plan.SyncListings(request.MovieIds);
    await planRepository.UpdateAsync(plan, ct);
    return true;
  }
}
