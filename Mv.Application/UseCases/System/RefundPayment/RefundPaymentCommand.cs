using Mv.Application.Abstractions;

namespace Mv.Application.UseCases.System.RefundPayment;

public record RefundPaymentCommand(Guid Id) : ICommand<bool>;
