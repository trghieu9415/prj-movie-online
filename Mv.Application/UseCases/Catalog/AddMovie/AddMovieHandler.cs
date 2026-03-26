using MediatR;
using Mv.Application.Repositories;
using Mv.Domain.Entities;

namespace Mv.Application.UseCases.Catalog.AddMovie;

public class AddMovieHandler(
  IRepository<Movie> movieRepository
) : IRequestHandler<AddMovieCommand, Guid> {
  public async Task<Guid> Handle(AddMovieCommand request, CancellationToken ct) {
    var movie = Movie.Create(request.Name, request.Duration, request.PosterUrl);
    return await movieRepository.CreateAsync(movie, ct);
  }
}
