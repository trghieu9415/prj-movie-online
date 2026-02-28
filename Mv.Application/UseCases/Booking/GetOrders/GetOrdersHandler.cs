using AutoMapper;
using Domain.Entities;
using MediatR;
using Mv.Application.DTOs;
using Mv.Application.Exceptions;
using Mv.Application.Models;
using Mv.Application.Ports.Repositories;
using Mv.Application.Ports.Security;

namespace Mv.Application.UseCases.Booking.GetOrders;

public class GetOrdersHandler(
  IReadRepository<Order> orderReadRepository,
  ICurrentUser currentUser,
  IMapper mapper
) : IRequestHandler<GetOrdersQuery, GetOrdersResult> {
  public async Task<GetOrdersResult> Handle(GetOrdersQuery request, CancellationToken ct) {
    if (currentUser.Role != UserRole.Admin) {
      throw new WorkflowException("Chỉ có người dùng quản trị mới có thể thực hiện hành vi này", 403);
    }

    var (total, orders) = await orderReadRepository.GetAsync(
      null, [x => x.Tickets], ct
    );

    var orderDtos = mapper.Map<List<OrderDto>>(orders);
    var meta = Meta.Create(request.Page, request.PageSize, total);
    return new GetOrdersResult(orderDtos, meta);
  }
}
