using AutoMapper;
using Domain.Entities;
using Mv.Application.DTOs;

namespace Mv.Application.Mappers;

public class BookingProfile : Profile {
  public BookingProfile() {
    CreateMap<Ticket, TicketDto>();
    CreateMap<Order, OrderDto>()
      .ForMember(dest => dest.Tickets, opt => opt.MapFrom(src => src.Tickets));
    CreateMap<Payment, PaymentDto>();
  }
}
