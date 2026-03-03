using Mv.Application.Abstractions;

namespace Mv.Application.UseCases.Catalog.RemoveMovie;

public record RemoveMovieCommand(Guid Id) : ICommand<bool>;
