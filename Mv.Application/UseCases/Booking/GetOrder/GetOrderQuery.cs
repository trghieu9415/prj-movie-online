using FluentValidation;
using Mv.Application.Abstractions;
using Mv.Application.DTOs;

namespace Mv.Application.UseCases.Booking.GetOrder;

public record GetOrderQuery(Guid Id) : IQuery<GetOrderResult>;

public class GetOrderValidator : AbstractValidator<GetOrderQuery> {
  public GetOrderValidator() {
    RuleFor(x => x.Id)
      .NotEmpty().WithMessage("Id không hợp lệ.");
  }
}

public record GetOrderResult(OrderDto Order);
