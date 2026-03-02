using Domain.Entities;
using MediatR;
using Mv.Application.Repositories;

namespace Mv.Application.UseCases.Facility.RemoveAuditorium;

public class RemoveAuditoriumHandler(IRepository<Auditorium> auditoriumRepository)
  : IRequestHandler<RemoveAuditoriumCommand, bool> {
  public async Task<bool> Handle(RemoveAuditoriumCommand request, CancellationToken ct) {
    await auditoriumRepository.DeleteAsync(request.Id, true, ct);
    return true;
  }
}
