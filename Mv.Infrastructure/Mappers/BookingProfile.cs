using AutoMapper;
using Mv.Application.DTOs;
using Mv.Domain.Entities;

namespace Mv.Infrastructure.Mappers;

public class BookingProfile : Profile {
  public BookingProfile() {
    CreateMap<Ticket, TicketDto>();
    CreateMap<Order, OrderDto>()
      .ForMember(dest => dest.Tickets, opt => opt.MapFrom(src => src.Tickets));
    CreateMap<Payment, PaymentDto>();
  }
}
