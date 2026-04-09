using FluentValidation;
using Mv.Application.Abstractions;

namespace Mv.Application.UseCases.Catalog.UpdateMovie;

public record UpdateMovieCommand(Guid Id, string Name, int Duration, string PosterUrl) : ICommand<bool>;

public class UpdateMovieValidator : AbstractValidator<UpdateMovieCommand> {
  public UpdateMovieValidator() {
    RuleFor(x => x.Id)
      .NotEmpty().WithMessage("Id không hợp lệ.");

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
