using Domain.Entities;
using MediatR;
using Mv.Application.DTOs;
using Mv.Application.Exceptions;
using Mv.Application.Repositories;

namespace Mv.Application.UseCases.Facility.GetAuditorium;

public class GetAuditoriumHandler(
  IReadRepository<Auditorium, AuditoriumDto> auditoriumReadRepository
) : IRequestHandler<GetAuditoriumQuery, GetAuditoriumResult> {
  public async Task<GetAuditoriumResult> Handle(GetAuditoriumQuery request, CancellationToken ct) {
    var auditoriumDto =
      await auditoriumReadRepository.GetByIdAsync(request.Id, ct)
      ?? throw new WorkflowException("Phòng chiếu không tồn tại", 404);

    return new GetAuditoriumResult(auditoriumDto);
  }
}
