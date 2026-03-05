using Domain.Entities;
using MediatR;
using Mv.Application.DTOs;
using Mv.Application.Exceptions;
using Mv.Application.Models;
using Mv.Application.Ports.Security;
using Mv.Application.Repositories;

namespace Mv.Application.UseCases.Booking.GetOrder;

public class GetOrderHandler(
  IReadRepository<Order, OrderDto> orderReadRepository,
  ICurrentUser currentUser
) : IRequestHandler<GetOrderQuery, GetOrderResult> {
  public async Task<GetOrderResult> Handle(GetOrderQuery request, CancellationToken ct) {
    const string errorMessage = "Không tìm thấy đơn hàng hoặc bạn không có quyền truy cập";
    var orderDto =
      await orderReadRepository.GetByIdAsync(request.Id, ct)
      ?? throw new WorkflowException(errorMessage, 404);


    if (orderDto.CustomerId != currentUser.Id && currentUser.Role != UserRole.Admin) {
      throw new WorkflowException(errorMessage, 403);
    }

    return new GetOrderResult(orderDto);
  }
}
