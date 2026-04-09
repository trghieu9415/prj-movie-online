using FluentValidation;
using Mv.Application.Abstractions;

namespace Mv.Application.UseCases.Booking.ProcessPayment;

public record ProcessPaymentCommand(Guid Id, object Payload) : ITransactional, ICommand<bool>;

public class ProcessPaymentValidator : AbstractValidator<ProcessPaymentCommand> {
  public ProcessPaymentValidator() {
    RuleFor(x => x.Id)
      .NotEmpty().WithMessage("Id không hợp lệ.");

    RuleFor(x => x.Payload)
      .NotNull().WithMessage("Payload không được null.");
  }
}
