using Domain.Entities;
using MediatR;
using Mv.Application.Exceptions;
using Mv.Application.Ports.Security;
using Mv.Application.Repositories;

namespace Mv.Application.UseCases.Booking.CancelOrder;

public class CancelOrderHandler(
  IRepository<Order> orderRepository,
  ICurrentUser currentUser
) : IRequestHandler<CancelOrderCommand, bool> {
  public async Task<bool> Handle(CancelOrderCommand request, CancellationToken ct) {
    var order =
      await orderRepository.GetByIdAsync(request.Id, ct)
      ?? throw new WorkflowException("Đơn hàng không tồn tại", 404);

    if (currentUser.Id != order.CustomerId) {
      throw new WorkflowException("Đơn hàng không thuộc về người dùng hiện tại", 403);
    }

    order.Cancel();
    await orderRepository.UpdateAsync(order, ct);
    return true;
  }
}
