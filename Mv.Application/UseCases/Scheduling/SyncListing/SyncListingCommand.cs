using Mv.Application.Abstractions;

namespace Mv.Application.UseCases.Scheduling.SyncListing;

public record SyncListingCommand(Guid Id, List<Guid> MovieIds) : ICommand<bool>;
