using MediatR;
using Mv.Application.DTOs;
using Mv.Application.Models;
using Mv.Application.Repositories;
using Mv.Domain.Entities;

namespace Mv.Application.UseCases.Catalog.GetMovies;

public class GetMoviesHandler(
  IReadRepository<Movie, MovieDto> movieReadRepository
) : IRequestHandler<GetMoviesQuery, GetMoviesResult> {
  public async Task<GetMoviesResult> Handle(GetMoviesQuery request, CancellationToken ct) {
    var (total, movieDtos) = await movieReadRepository.GetAsync(
      page: request.Page,
      pageSize: request.PageSize,
      ct: ct
    );
    var meta = Meta.Create(request.Page, request.PageSize, total);
    return new GetMoviesResult(movieDtos, meta);
  }
}
