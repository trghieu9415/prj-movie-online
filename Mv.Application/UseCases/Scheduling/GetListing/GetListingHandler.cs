using MediatR;
using Mv.Application.Exceptions;
using Mv.Application.Repositories.Read;

namespace Mv.Application.UseCases.Scheduling.GetListing;

public class GetListingHandler(
  IListingReadRepository listingReadRepository
) : IRequestHandler<GetListingQuery, GetListingResult> {
  public async Task<GetListingResult> Handle(GetListingQuery request, CancellationToken ct) {
    var listingDto =
      await listingReadRepository.GetListingDetailsAsync(request.Id, ct)
      ?? throw new WorkflowException("Liệt kê chiếu phim không tồn tại", 404);

    return new GetListingResult(listingDto);
  }
}
