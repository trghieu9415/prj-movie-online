using AutoMapper;
using Domain.Entities;
using MediatR;
using Mv.Application.DTOs;
using Mv.Application.Exceptions;
using Mv.Application.Models;
using Mv.Application.Ports.Repositories;
using Mv.Application.Ports.Security;

namespace Mv.Application.UseCases.Booking.GetOrder;

public class GetOrderHandler(
  IReadRepository<Order> orderReadRepository,
  ICurrentUser currentUser,
  IMapper mapper
) : IRequestHandler<GetOrderQuery, GetOrderResult> {
  public async Task<GetOrderResult> Handle(GetOrderQuery request, CancellationToken ct) {
    var order =
      await orderReadRepository.GetByIdAsync(request.OrderId, ct)
      ?? throw new WorkflowException("Không tìm thấy đơn hàng", 404);

    if (currentUser.Role != UserRole.Admin || order.CustomerId != currentUser.Id) {
      throw new WorkflowException("Đơn hàng không thuộc về người dùng hiện tại");
    }

    return new GetOrderResult(mapper.Map<OrderDto>(order));
  }
}
