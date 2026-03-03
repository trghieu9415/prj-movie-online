using Mv.Application.Abstractions;

namespace Mv.Application.UseCases.System.ExpireOrder;

public record ExpireOrderCommand(Guid Id) : ICommand<bool>;
