using FluentValidation;
using Mv.Application.Abstractions;

namespace Mv.Application.UseCases.Catalog.AddMovie;

public record AddMovieCommand(string Name, int Duration, string PosterUrl) : ICommand<Guid>;

public class AddMovieValidator : AbstractValidator<AddMovieCommand> {
  public AddMovieValidator() {
    RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
    RuleFor(x => x.Duration).GreaterThan(0).WithMessage("Thời lượng phải lớn hơn 0");
    RuleFor(x => x.PosterUrl).NotEmpty();
  }
}
