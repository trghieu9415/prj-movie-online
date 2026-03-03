using Mv.Application.Abstractions;

namespace Mv.Application.UseCases.System.RefundOrder;

public record RefundOrderCommand(Guid Id) : ICommand<bool>;
