using Mv.Application.Abstractions;
using Mv.Application.DTOs;
using Mv.Application.Models;

namespace Mv.Application.UseCases.Booking.GetHistory;

public record GetHistoryQuery(int Page = 1, int PageSize = 10) : IQuery<GetHistoryResult>;

public record GetHistoryResult(List<OrderDto> Orders, Meta Meta);
