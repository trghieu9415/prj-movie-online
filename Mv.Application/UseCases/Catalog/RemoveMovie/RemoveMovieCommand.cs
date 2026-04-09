using FluentValidation;
using Mv.Application.Abstractions;

namespace Mv.Application.UseCases.Catalog.RemoveMovie;

public record RemoveMovieCommand(Guid Id) : ICommand<bool>;

public class RemoveMovieValidator : AbstractValidator<RemoveMovieCommand> {
  public RemoveMovieValidator() {
    RuleFor(x => x.Id)
      .NotEmpty().WithMessage("Id không hợp lệ.");
  }
}
