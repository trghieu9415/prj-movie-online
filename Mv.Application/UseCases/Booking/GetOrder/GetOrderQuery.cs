using Mv.Application.Abstractions;
using Mv.Application.DTOs;
using Mv.Application.Models;

namespace Mv.Application.UseCases.Booking.GetOrder;

public record GetOrderQuery(Guid OrderId) : IQuery<GetOrderResult>;

public record GetOrderResult(OrderDto Order);
