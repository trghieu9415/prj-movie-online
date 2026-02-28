using AutoMapper;
using Domain.Entities;
using MediatR;
using Mv.Application.DTOs;
using Mv.Application.Models;
using Mv.Application.Ports.Repositories;
using Mv.Application.Ports.Security;

namespace Mv.Application.UseCases.Booking.GetHistory;

public class GetHistoryHandler(
  IReadRepository<Order> orderReadRepository,
  ICurrentUser currentUser,
  IMapper mapper
) : IRequestHandler<GetHistoryQuery, GetHistoryResult> {
  public async Task<GetHistoryResult> Handle(GetHistoryQuery request, CancellationToken ct) {
    var (total, orders) = await orderReadRepository.GetAsync(
      x => x.CustomerId == currentUser.Id,
      [x => x.Tickets],
      ct
    );

    var orderDtos = mapper.Map<List<OrderDto>>(orders);
    var meta = Meta.Create(request.Page, request.PageSize, total);
    return new GetHistoryResult(orderDtos, meta);
  }
}
