using Domain.Entities;
using MediatR;
using Mv.Application.Exceptions;
using Mv.Application.Ports.Security;
using Mv.Application.Repositories;

namespace Mv.Application.UseCases.Booking.PlaceOrder;

public class PlaceOrderHandler(
  IRepository<Order> orderRepository,
  IRepository<Showtime> showtimeRepository,
  IRepository<Auditorium> auditoriumRepository,
  IRepository<Seat> seatRepository,
  ICurrentUser currentUser
) : IRequestHandler<PlaceOrderCommand, Guid> {
  public async Task<Guid> Handle(PlaceOrderCommand request, CancellationToken ct) {
    var showtime =
      await showtimeRepository.GetByIdAsync(request.ShowtimeId, ct)
      ?? throw new WorkflowException($"Suất chiếu không tồn tại - Id: {request.ShowtimeId}", 404);

    var auditorium =
      await auditoriumRepository.GetByIdAsync(showtime.AuditoriumId, ct)
      ?? throw new Exception(
        $"Rạp phim không tồn tại - ShowtimeId: {showtime.Id} / AuditoriumId: {showtime.AuditoriumId}"
      );

    if (request.SeatIds.Any(seatId => auditorium.Seats.All(s => s.Id != seatId))) {
      throw new WorkflowException($"Ghế không tồn tại trong rạp {auditorium.Name}");
    }

    var seats = await seatRepository.GetByKeysAsync(request.SeatIds, null, ct);
    if (seats.Count != request.SeatIds.Count) {
      throw new WorkflowException("Một số ghế bạn chọn không tồn tại");
    }

    var order = Order.Create(
      currentUser.Id,
      currentUser.FullName,
      showtime.Id,
      auditorium.Name
    );

    order.SyncTickets(seats.Select(s => s.ToSnapshot()).ToList());
    await orderRepository.CreateAsync(order, ct);
    return order.Id;
  }
}
