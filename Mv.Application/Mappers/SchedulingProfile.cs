using AutoMapper;
using Domain.Entities;
using Mv.Application.DTOs;

namespace Mv.Application.Mappers;

public class SchedulingProfile : Profile {
  public SchedulingProfile() {
    CreateMap<Showtime, ShowtimeDto>();
    CreateMap<Plan, PlanDto>()
      .ForMember(dest => dest.Showtimes, opt => opt.MapFrom(src => src.Showtimes));
  }
}
