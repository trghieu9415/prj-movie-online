using Domain.Entities;
using MediatR;
using Mv.Application.DTOs;
using Mv.Application.Exceptions;
using Mv.Application.Models;
using Mv.Application.Ports.Security;
using Mv.Application.Repositories;

namespace Mv.Application.UseCases.Booking.GetOrders;

public class GetOrdersHandler(
  IReadRepository<Order, OrderDto> orderReadRepository,
  ICurrentUser currentUser
) : IRequestHandler<GetOrdersQuery, GetOrdersResult> {
  public async Task<GetOrdersResult> Handle(GetOrdersQuery request, CancellationToken ct) {
    if (currentUser.Role != UserRole.Admin) {
      throw new WorkflowException("Chỉ có người dùng quản trị mới có thể thực hiện hành vi này", 403);
    }

    var (total, orderDtos) = await orderReadRepository.GetAsync(
      page: request.Page, pageSize: request.PageSize, ct: ct
    );

    var meta = Meta.Create(request.Page, request.PageSize, total);
    return new GetOrdersResult(orderDtos, meta);
  }
}
