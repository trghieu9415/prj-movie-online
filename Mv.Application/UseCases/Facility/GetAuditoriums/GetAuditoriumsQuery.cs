using FluentValidation;
using Mv.Application.Abstractions;
using Mv.Application.DTOs;
using Mv.Application.Models;

namespace Mv.Application.UseCases.Facility.GetAuditoriums;

public record GetAuditoriumsQuery(int Page = 1, int PageSize = 10) : IQuery<GetAuditoriumsResult>;

public record GetAuditoriumsResult(List<AuditoriumDto> Auditoriums, Meta Meta);

public class GetAuditoriumsValidator : AbstractValidator<GetAuditoriumsQuery> {
  public GetAuditoriumsValidator() {
    RuleFor(x => x.Page)
      .GreaterThan(0).WithMessage("Page phải lớn hơn 0.");

    RuleFor(x => x.PageSize)
      .GreaterThan(0).WithMessage("PageSize phải lớn hơn 0.")
      .LessThanOrEqualTo(100).WithMessage("PageSize không được vượt quá 100.");
  }
}
