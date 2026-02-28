using Mv.Application.Abstractions;
using Mv.Application.DTOs;

namespace Mv.Application.UseCases.Catalog.GetMovie;

public record GetMovieQuery(Guid Id) : IQuery<GetMovieResult>;

public record GetMovieResult(MovieDto Movie);
