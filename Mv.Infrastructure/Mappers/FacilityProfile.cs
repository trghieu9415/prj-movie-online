using AutoMapper;
using Mv.Application.DTOs;
using Mv.Domain.Entities;

namespace Mv.Infrastructure.Mappers;

public class FacilityProfile : Profile {
  public FacilityProfile() {
    CreateMap<Seat, SeatDto>();
    CreateMap<Auditorium, AuditoriumDto>()
      .ForMember(dest => dest.Seats, opt => opt.MapFrom(src => src.Seats));
  }
}
