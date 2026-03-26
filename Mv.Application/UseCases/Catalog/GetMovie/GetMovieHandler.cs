using MediatR;
using Mv.Application.DTOs;
using Mv.Application.Exceptions;
using Mv.Application.Repositories;
using Mv.Domain.Entities;

namespace Mv.Application.UseCases.Catalog.GetMovie;

public class GetMovieHandler(
  IReadRepository<Movie, MovieDto> movieReadRepository
) : IRequestHandler<GetMovieQuery, GetMovieResult> {
  public async Task<GetMovieResult> Handle(GetMovieQuery request, CancellationToken ct) {
    var movieDto =
      await movieReadRepository.GetByIdAsync(request.Id, ct)
      ?? throw new WorkflowException("Phim không tồn tại", 404);

    return new GetMovieResult(movieDto);
  }
}
