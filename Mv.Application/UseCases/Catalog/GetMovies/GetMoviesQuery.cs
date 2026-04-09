using FluentValidation;
using Mv.Application.Abstractions;
using Mv.Application.DTOs;
using Mv.Application.Models;

namespace Mv.Application.UseCases.Catalog.GetMovies;

public record GetMoviesQuery(int Page = 1, int PageSize = 10) : IQuery<GetMoviesResult>;

public class GetMoviesValidator : AbstractValidator<GetMoviesQuery> {
  public GetMoviesValidator() {
    RuleFor(x => x.Page)
      .GreaterThan(0).WithMessage("Page phải lớn hơn 0.");

    RuleFor(x => x.PageSize)
      .GreaterThan(0).WithMessage("PageSize phải lớn hơn 0.")
      .LessThanOrEqualTo(100).WithMessage("PageSize không được vượt quá 100.");
  }
}

public record GetMoviesResult(List<MovieDto> Movies, Meta Meta);
