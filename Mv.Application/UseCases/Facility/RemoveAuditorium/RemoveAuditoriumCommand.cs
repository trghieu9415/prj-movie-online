using Mv.Application.Abstractions;

namespace Mv.Application.UseCases.Facility.RemoveAuditorium;

public record RemoveAuditoriumCommand(Guid Id) : ICommand<bool>;
