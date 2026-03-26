using MediatR;
using Mv.Application.DTOs;
using Mv.Application.Models;
using Mv.Application.Repositories;
using Mv.Domain.Entities;

namespace Mv.Application.UseCases.Facility.GetAuditoriums;

public class GetAuditoriumsHandler(
  IReadRepository<Auditorium, AuditoriumDto> auditoriumReadRepository
) : IRequestHandler<GetAuditoriumsQuery, GetAuditoriumsResult> {
  public async Task<GetAuditoriumsResult> Handle(GetAuditoriumsQuery request, CancellationToken ct) {
    var (total, auditoriumDtos) = await auditoriumReadRepository.GetAsync(
      page: request.Page,
      pageSize: request.PageSize,
      ct: ct
    );
    var meta = Meta.Create(request.Page, request.PageSize, total);
    return new GetAuditoriumsResult(auditoriumDtos, meta);
  }
}
