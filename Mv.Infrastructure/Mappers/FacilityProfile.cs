using AutoMapper;
using Domain.Entities;
using Mv.Application.DTOs;

namespace Mv.Infrastructure.Mappers;

public class FacilityProfile : Profile {
  public FacilityProfile() {
    CreateMap<Seat, SeatDto>();
    CreateMap<Auditorium, AuditoriumDto>()
      .ForMember(dest => dest.Seats, opt => opt.MapFrom(src => src.Seats));
  }
}
