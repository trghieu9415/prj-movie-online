using MediatR;
using Mv.Application.Repositories;
using Mv.Domain.Entities;

namespace Mv.Application.UseCases.Facility.AddAuditorium;

public class AddAuditoriumHandler(
  IRepository<Auditorium> auditoriumRepository
) : IRequestHandler<AddAuditoriumCommand, Guid> {
  public async Task<Guid> Handle(AddAuditoriumCommand request, CancellationToken ct) {
    var auditorium = Auditorium.Create(request.Name);
    auditorium.GenerateSeatMatrix(request.RowCount, request.SeatsPerRow);
    return await auditoriumRepository.CreateAsync(auditorium, ct);
  }
}
