using Domain.Entities;
using MediatR;
using Mv.Application.Repositories;

namespace Mv.Application.UseCases.Catalog.RemoveMovie;

public class RemoveMovieHandler(IRepository<Movie> movieRepository)
  : IRequestHandler<RemoveMovieCommand, bool> {
  public async Task<bool> Handle(RemoveMovieCommand request, CancellationToken ct) {
    await movieRepository.DeleteAsync(request.Id, true, ct);
    return true;
  }
}
