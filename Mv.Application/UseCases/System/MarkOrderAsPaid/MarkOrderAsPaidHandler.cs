using MediatR;
using Mv.Application.Exceptions;
using Mv.Application.Repositories;
using Mv.Domain.Entities;

namespace Mv.Application.UseCases.System.MarkOrderAsPaid;

public class MarkOrderAsPaidHandler(
  IRepository<Order> orderRepository
) : IRequestHandler<MarkOrderAsPaidCommand, bool> {
  public async Task<bool> Handle(MarkOrderAsPaidCommand request, CancellationToken ct) {
    var order =
      await orderRepository.GetByIdAsync(request.Id, ct)
      ?? throw new WorkflowException("Đơn hàng không tồn tại", 404);

    order.MarkAsPaid();

    await orderRepository.UpdateAsync(order, ct);
    return true;
  }
}
