using Mv.Application.Abstractions;

namespace Mv.Application.UseCases.System.MarkOrderAsPaid;

public record MarkOrderAsPaidCommand(Guid Id) : ICommand<bool>;
