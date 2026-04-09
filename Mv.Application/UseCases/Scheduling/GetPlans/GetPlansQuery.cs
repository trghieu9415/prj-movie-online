using FluentValidation;
using Mv.Application.Abstractions;
using Mv.Application.DTOs;
using Mv.Application.Models;

namespace Mv.Application.UseCases.Scheduling.GetPlans;

public record GetPlansQuery(int Page = 1, int PageSize = 10) : IQuery<GetPlansResult>;

public record GetPlansResult(List<PlanDto> Plans, Meta Meta);

public class GetPlansValidator : AbstractValidator<GetPlansQuery> {
  public GetPlansValidator() {
    RuleFor(x => x.Page)
      .GreaterThan(0).WithMessage("Page phải lớn hơn 0.");

    RuleFor(x => x.PageSize)
      .GreaterThan(0).WithMessage("PageSize phải lớn hơn 0.")
      .LessThanOrEqualTo(100).WithMessage("PageSize không được vượt quá 100.");
  }
}
