using MediatR;
using Mv.Application.Repositories;
using Mv.Domain.Entities;

namespace Mv.Application.UseCases.Catalog.RemoveMovie;

public class RemoveMovieHandler(IRepository<Movie> movieRepository)
  : IRequestHandler<RemoveMovieCommand, bool> {
  public async Task<bool> Handle(RemoveMovieCommand request, CancellationToken ct) {
    await movieRepository.DeleteAsync(request.Id, true, ct);
    return true;
  }
}
