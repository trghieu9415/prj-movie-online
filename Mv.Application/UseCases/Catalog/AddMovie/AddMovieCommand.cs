using FluentValidation;
using Mv.Application.Abstractions;

namespace Mv.Application.UseCases.Catalog.AddMovie;

public record AddMovieCommand(string Name, int Duration, string PosterUrl) : ICommand<Guid>;

public class AddMovieValidator : AbstractValidator<AddMovieCommand> {
  public AddMovieValidator() {
    RuleFor(x => x.Name)
      .NotEmpty().WithMessage("Tên phim không được để trống.")
      .MaximumLength(200).WithMessage("Tên phim không được vượt quá 200 ký tự.");

    RuleFor(x => x.Duration)
      .GreaterThan(0).WithMessage("Thời lượng phim phải lớn hơn 0.");

    RuleFor(x => x.PosterUrl)
      .NotEmpty().WithMessage("PosterUrl không được để trống.")
      .Must(url => Uri.TryCreate(url, UriKind.Absolute, out _))
      .WithMessage("PosterUrl không hợp lệ.");
  }
}
