using FluentValidation;
using Mv.Application.Abstractions;

namespace Mv.Application.UseCases.Scheduling.GetLockedSeatQuery;

public record GetLockedSeatQuery(Guid Id) : IQuery<GetLockedSeatResult>;

public record GetLockedSeatResult(List<Guid> SeatIds);

public class GetLockedSeatValidator : AbstractValidator<GetLockedSeatQuery> {
  public GetLockedSeatValidator() {
    RuleFor(x => x.Id)
      .NotEmpty().WithMessage("Id không hợp lệ.");
  }
}
