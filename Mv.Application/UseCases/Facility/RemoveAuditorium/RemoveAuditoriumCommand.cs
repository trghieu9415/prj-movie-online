using FluentValidation;
using Mv.Application.Abstractions;

namespace Mv.Application.UseCases.Facility.RemoveAuditorium;

public record RemoveAuditoriumCommand(Guid Id) : ICommand<bool>;

public class RemoveAuditoriumValidator : AbstractValidator<RemoveAuditoriumCommand> {
  public RemoveAuditoriumValidator() {
    RuleFor(x => x.Id)
      .NotEmpty().WithMessage("Id không hợp lệ.");
  }
}
