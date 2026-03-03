using Mv.Application.Abstractions;
using Mv.Application.DTOs;

namespace Mv.Application.UseCases.Booking.GetOrder;

public record GetOrderQuery(Guid Id) : IQuery<GetOrderResult>;

public record GetOrderResult(OrderDto Order);
