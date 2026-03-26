using MediatR;
using Mv.Application.Exceptions;
using Mv.Application.Repositories;
using Mv.Domain.Entities;

namespace Mv.Application.UseCases.Facility.UpdateAuditorium;

public class UpdateAuditoriumHandler(IRepository<Auditorium> auditoriumRepository)
  : IRequestHandler<UpdateAuditoriumCommand, bool> {
  public async Task<bool> Handle(UpdateAuditoriumCommand request, CancellationToken ct) {
    var auditorium =
      await auditoriumRepository.GetByIdAsync(request.Id, ct)
      ?? throw new WorkflowException("Phòng chiếu không tồn tại", 404);

    auditorium.Update(request.Name);
    await auditoriumRepository.UpdateAsync(auditorium, ct);
    return true;
  }
}
