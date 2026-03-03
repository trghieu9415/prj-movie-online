using MediatR;
using Mv.Application.Exceptions;
using Mv.Application.Ports.Cache;

namespace Mv.Application.UseCases.Facility.GetAuditorium;

public class GetAuditoriumHandler(
  IBusinessCache businessCache
) : IRequestHandler<GetAuditoriumQuery, GetAuditoriumResult> {
  public async Task<GetAuditoriumResult> Handle(GetAuditoriumQuery request, CancellationToken ct) {
    var auditoriumDto =
      await businessCache.GetAuditoriumAsync(request.Id, ct)
      ?? throw new WorkflowException("Phòng chiếu không tồn tại", 404);

    return new GetAuditoriumResult(auditoriumDto);
  }
}
