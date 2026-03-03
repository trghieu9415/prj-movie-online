using MediatR;
using Mv.Application.Exceptions;
using Mv.Application.Ports.Cache;

namespace Mv.Application.UseCases.Scheduling.GetListing;

public class GetListingHandler(
  IBusinessCache businessCache
) : IRequestHandler<GetListingQuery, GetListingResult> {
  public async Task<GetListingResult> Handle(GetListingQuery request, CancellationToken ct) {
    var listingDto =
      await businessCache.GetListingAsync(request.Id, ct)
      ?? throw new WorkflowException("Lịch chiếu phim không tồn tại", 404);

    return new GetListingResult(listingDto);
  }
}
