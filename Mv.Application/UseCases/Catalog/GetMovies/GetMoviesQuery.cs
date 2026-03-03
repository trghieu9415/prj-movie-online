using Mv.Application.Abstractions;
using Mv.Application.DTOs;
using Mv.Application.Models;

namespace Mv.Application.UseCases.Catalog.GetMovies;

public record GetMoviesQuery(int Page = 1, int PageSize = 10) : IQuery<GetMoviesResult>;

public record GetMoviesResult(List<MovieDto> Movies, Meta Meta);
