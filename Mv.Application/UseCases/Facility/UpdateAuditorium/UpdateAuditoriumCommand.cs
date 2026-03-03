using FluentValidation;
using Mv.Application.Abstractions;

namespace Mv.Application.UseCases.Facility.UpdateAuditorium;

public record UpdateAuditoriumCommand(Guid Id, string Name) : ICommand<bool>;

public class UpdateAuditoriumValidator : AbstractValidator<UpdateAuditoriumCommand> {
  public UpdateAuditoriumValidator() {
    RuleFor(x => x.Id).NotEmpty();
    RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
  }
}
