using FluentValidation;
using Mv.Application.Abstractions;
using Mv.Application.DTOs;

namespace Mv.Application.UseCases.Catalog.GetMovie;

public record GetMovieQuery(Guid Id) : IQuery<GetMovieResult>;

public class GetMovieValidator : AbstractValidator<GetMovieQuery> {
  public GetMovieValidator() {
    RuleFor(x => x.Id)
      .NotEmpty().WithMessage("Id không hợp lệ.");
  }
}

public record GetMovieResult(MovieDto Movie);
