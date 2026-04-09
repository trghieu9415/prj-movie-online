using Mv.Application.Abstractions;

namespace Mv.Application.UseCases.System.ExpireOrder;

public record ExpireOrderCommand(Guid Id) : ITransactional, ICommand<bool>;
