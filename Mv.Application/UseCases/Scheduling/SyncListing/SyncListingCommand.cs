using FluentValidation;
using Mv.Application.Abstractions;

namespace Mv.Application.UseCases.Scheduling.SyncListing;

public record SyncListingCommand(Guid Id, List<Guid> MovieIds) : ICommand<bool>;

public class SyncListingValidator : AbstractValidator<SyncListingCommand> {
  public SyncListingValidator() {
    RuleFor(x => x.Id)
      .NotEmpty().WithMessage("Id không hợp lệ.");

    RuleFor(x => x.MovieIds)
      .NotNull().WithMessage("Danh sách MovieIds không được null.")
      .Must(x => x.Count > 0).WithMessage("Danh sách MovieIds phải có ít nhất 1 phần tử.");

    RuleForEach(x => x.MovieIds)
      .NotEmpty().WithMessage("MovieId không hợp lệ.");
  }
}
