using AutoMapper;
using Domain.Entities;
using MediatR;
using Mv.Application.DTOs;
using Mv.Application.Exceptions;
using Mv.Application.Ports.Repositories;

namespace Mv.Application.UseCases.Catalog.GetMovie;

public class GetMovieHandler(IReadRepository<Movie> movieReadRepository, IMapper mapper)
  : IRequestHandler<GetMovieQuery, GetMovieResult> {
  public async Task<GetMovieResult> Handle(GetMovieQuery request, CancellationToken ct) {
    var movie =
      await movieReadRepository.GetByIdAsync(request.Id, ct)
      ?? throw new WorkflowException("Phim không tồn tại", 404);

    var movieDto = mapper.Map<MovieDto>(movie);
    return new GetMovieResult(movieDto);
  }
}
