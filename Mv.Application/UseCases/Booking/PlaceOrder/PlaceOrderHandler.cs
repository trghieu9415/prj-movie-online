using MediatR;
using Mv.Application.Exceptions;
using Mv.Application.Ports.Security;
using Mv.Application.Repositories;
using Mv.Application.Repositories.Read;
using Mv.Domain.Entities;

namespace Mv.Application.UseCases.Booking.PlaceOrder;

public class PlaceOrderHandler(
  IRepository<Order> orderRepository,
  IShowtimeReadRepository showtimeRepository,
  ISeatReadRepository seatRepository,
  ICurrentUser currentUser
) : IRequestHandler<PlaceOrderCommand, Guid> {
  public async Task<Guid> Handle(PlaceOrderCommand request, CancellationToken ct) {
    var showtime =
      await showtimeRepository.GetByIdAsync(request.ShowtimeId, ct)
      ?? throw new WorkflowException($"Suất chiếu không tồn tại - Id: {request.ShowtimeId}", 404);

    var validSeats = await seatRepository.GetValidSeatsForShowtimeAsync(
      request.SeatIds, request.ShowtimeId, ct
    );

    if (validSeats.Count != request.SeatIds.Count) {
      throw new WorkflowException("Tồn tại ghế không phù hợp hoặc ghế đã được đặt");
    }

    var (movie, auditoriumName) =
      await showtimeRepository.GetMovieAndAuditoriumAsync(showtime.Id, ct)
      ?? throw new WorkflowException("Suất chiếu chứa phim không tồn tại");

    var order = Order.Create(
      currentUser.Id,
      currentUser.FullName,
      showtime.Id,
      auditoriumName,
      movie,
      validSeats
    );

    await orderRepository.CreateAsync(order, ct);
    return order.Id;
  }
}
