using AutoMapper;
using Domain.Entities;
using MediatR;
using Mv.Application.DTOs;
using Mv.Application.Models;
using Mv.Application.Ports.Repositories;

namespace Mv.Application.UseCases.Catalog.GetMovies;

public class GetMoviesHandler(IReadRepository<Movie> movieReadRepo, IMapper mapper)
  : IRequestHandler<GetMoviesQuery, GetMoviesResult> {
  public async Task<GetMoviesResult> Handle(GetMoviesQuery request, CancellationToken ct) {
    var (total, movies) = await movieReadRepo.GetAsync(ct: ct);
    var meta = Meta.Create(request.Page, request.PageSize, total);
    var movieDtos = mapper.Map<List<MovieDto>>(movies);
    return new GetMoviesResult(movieDtos, meta);
  }
}
