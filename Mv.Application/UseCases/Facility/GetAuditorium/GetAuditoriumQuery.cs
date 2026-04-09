using FluentValidation;
using Mv.Application.Abstractions;
using Mv.Application.DTOs;

namespace Mv.Application.UseCases.Facility.GetAuditorium;

public record GetAuditoriumQuery(Guid Id) : IQuery<GetAuditoriumResult>;

public record GetAuditoriumResult(AuditoriumDto Auditorium);

public class GetAuditoriumValidator : AbstractValidator<GetAuditoriumQuery> {
  public GetAuditoriumValidator() {
    RuleFor(x => x.Id)
      .NotEmpty().WithMessage("Id không hợp lệ.");
  }
}
