using Mv.Application.Abstractions;
using Mv.Application.DTOs;
using Mv.Application.Models;

namespace Mv.Application.UseCases.Facility.GetAuditoriums;

public record GetAuditoriumsQuery(int Page = 1, int PageSize = 10) : IQuery<GetAuditoriumsResult>;

public record GetAuditoriumsResult(List<AuditoriumDto> Auditoriums, Meta Meta);
