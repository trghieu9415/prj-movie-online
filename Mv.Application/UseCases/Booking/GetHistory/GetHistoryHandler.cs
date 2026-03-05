using Domain.Entities;
using MediatR;
using Mv.Application.DTOs;
using Mv.Application.Models;
using Mv.Application.Ports.Security;
using Mv.Application.Repositories;

namespace Mv.Application.UseCases.Booking.GetHistory;

public class GetHistoryHandler(
  IReadRepository<Order, OrderDto> orderReadRepository,
  ICurrentUser currentUser
) : IRequestHandler<GetHistoryQuery, GetHistoryResult> {
  public async Task<GetHistoryResult> Handle(GetHistoryQuery request, CancellationToken ct) {
    var (total, orderDtos) = await orderReadRepository.GetAsync(
      x => x.CustomerId == currentUser.Id,
      page: request.Page,
      pageSize: request.PageSize,
      ct: ct
    );
    var meta = Meta.Create(request.Page, request.PageSize, total);
    return new GetHistoryResult(orderDtos, meta);
  }
}
