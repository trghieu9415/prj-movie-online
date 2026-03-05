using Domain.Entities;
using MediatR;
using Mv.Application.Exceptions;
using Mv.Application.Repositories;

namespace Mv.Application.UseCases.System.RefundOrder;

public class RefundOrderHandler(
  IRepository<Order> orderRepository
) : IRequestHandler<RefundOrderCommand, bool> {
  public async Task<bool> Handle(RefundOrderCommand request, CancellationToken ct) {
    var order =
      await orderRepository.GetByIdAsync(request.Id, ct)
      ?? throw new WorkflowException("Đơn hàng không tồn tại", 404);

    order.Refund();
    await orderRepository.UpdateAsync(order, ct);
    return true;
  }
}
