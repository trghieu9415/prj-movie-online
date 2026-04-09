using FluentValidation;
using Mv.Application.Abstractions;
using Mv.Application.DTOs;
using Mv.Application.Models;

namespace Mv.Application.UseCases.Booking.GetOrders;

public record GetOrdersQuery(int Page = 1, int PageSize = 10) : IQuery<GetOrdersResult>;

public class GetOrdersValidator : AbstractValidator<GetOrdersQuery> {
  public GetOrdersValidator() {
    RuleFor(x => x.Page)
      .GreaterThan(0).WithMessage("Page phải lớn hơn 0.");

    RuleFor(x => x.PageSize)
      .GreaterThan(0).WithMessage("PageSize phải lớn hơn 0.")
      .LessThanOrEqualTo(100).WithMessage("PageSize không được vượt quá 100.");
  }
}

public record GetOrdersResult(List<OrderDto> Orders, Meta Meta);
