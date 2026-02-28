using Mv.Application.Abstractions;
using Mv.Application.DTOs;
using Mv.Application.Models;

namespace Mv.Application.UseCases.Booking.GetOrders;

public record GetOrdersQuery(int Page = 1, int PageSize = 10) : IQuery<GetOrdersResult>;

public record GetOrdersResult(List<OrderDto> Orders, Meta Meta);
