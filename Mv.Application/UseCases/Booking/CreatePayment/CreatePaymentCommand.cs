using FluentValidation;
using Mv.Application.Abstractions;
using Mv.Domain.Enums;

namespace Mv.Application.UseCases.Booking.CreatePayment;

public record CreatePaymentCommand(Guid OrderId, PaymentMethod Method) : ITransactional, ICommand<CreatePaymentResult>;

public record CreatePaymentResult(Guid PaymentId, string PaymentUrl);

public class CreatePaymentValidator : AbstractValidator<CreatePaymentCommand> {
  public CreatePaymentValidator() {
    RuleFor(x => x.OrderId)
      .NotEmpty().WithMessage("OrderId không hợp lệ.");

    RuleFor(x => x.Method)
      .IsInEnum().WithMessage("Phương thức thanh toán không hợp lệ.");
  }
}
