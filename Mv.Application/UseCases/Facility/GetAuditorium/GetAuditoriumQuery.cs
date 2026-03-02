using Mv.Application.Abstractions;
using Mv.Application.DTOs;

namespace Mv.Application.UseCases.Facility.GetAuditorium;

public record GetAuditoriumQuery(Guid Id) : IQuery<GetAuditoriumResult>;

public record GetAuditoriumResult(AuditoriumDto Auditorium);
