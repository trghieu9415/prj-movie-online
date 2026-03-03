using Mv.Application.Abstractions;

namespace Mv.Application.UseCases.Catalog.UpdateMovie;

public record UpdateMovieCommand(Guid Id, string Name, int Duration, string PosterUrl) : ICommand<bool>;
