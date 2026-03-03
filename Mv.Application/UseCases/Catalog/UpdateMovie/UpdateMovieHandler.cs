using Domain.Entities;
using MediatR;
using Mv.Application.Exceptions;
using Mv.Application.Repositories;

namespace Mv.Application.UseCases.Catalog.UpdateMovie;

public class UpdateMovieHandler(IRepository<Movie> movieRepository)
  : IRequestHandler<UpdateMovieCommand, bool> {
  public async Task<bool> Handle(UpdateMovieCommand request, CancellationToken ct) {
    var movie =
      await movieRepository.GetByIdAsync(request.Id, ct)
      ?? throw new WorkflowException("Không tìm thấy phim", 404);
    movie.Update(request.Name, request.Duration, request.PosterUrl);
    await movieRepository.UpdateAsync(movie, ct);
    return true;
  }
}
