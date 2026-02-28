using Mv.Application.Abstractions;

namespace Mv.Application.UseCases.Catalog.UpdateMovie;

public record UpdateMovieCommand(Guid MovieId, string Name, int Duration, string PosterUrl) : ICommand<bool>;
