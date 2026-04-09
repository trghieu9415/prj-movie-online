using FluentValidation;
using Mv.Application.Abstractions;
using Mv.Application.DTOs;
using Mv.Application.Models;

namespace Mv.Application.UseCases.Booking.GetHistory;

public record GetHistoryQuery(int Page = 1, int PageSize = 10) : IQuery<GetHistoryResult>;

public record GetHistoryResult(List<OrderDto> Orders, Meta Meta);

public class GetHistoryValidator : AbstractValidator<GetHistoryQuery> {
  public GetHistoryValidator() {
    RuleFor(x => x.Page)
      .GreaterThan(0).WithMessage("Page phải lớn hơn 0.");

    RuleFor(x => x.PageSize)
      .GreaterThan(0).WithMessage("PageSize phải lớn hơn 0.")
      .LessThanOrEqualTo(100).WithMessage("PageSize không được vượt quá 100.");
  }
}
