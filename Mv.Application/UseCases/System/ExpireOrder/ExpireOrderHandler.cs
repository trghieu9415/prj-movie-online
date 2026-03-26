using MediatR;
using Mv.Application.Exceptions;
using Mv.Application.Repositories;
using Mv.Domain.Entities;
using Mv.Domain.Enums;

namespace Mv.Application.UseCases.System.ExpireOrder;

public class ExpireOrderHandler(
  IRepository<Order> orderRepository
) : IRequestHandler<ExpireOrderCommand, bool> {
  public async Task<bool> Handle(ExpireOrderCommand request, CancellationToken ct) {
    var order =
      await orderRepository.GetByIdAsync(request.Id, ct)
      ?? throw new WorkflowException("Đơn hàng không tồn tại", 404);

    if (order.Status == OrderStatus.Confirmed) {
      return false;
    }

    order.Cancel();
    await orderRepository.UpdateAsync(order, ct);
    return true;
  }
}
