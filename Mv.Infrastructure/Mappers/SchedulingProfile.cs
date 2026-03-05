using AutoMapper;
using Domain.Entities;
using Mv.Application.DTOs;

namespace Mv.Infrastructure.Mappers;

public class SchedulingProfile : Profile {
  public SchedulingProfile() {
    CreateMap<Showtime, ShowtimeDto>();
    CreateMap<Listing, ListingDto>()
      .ForMember(dest => dest.Movie, opt => opt.MapFrom(src => new MovieDto {
        Id = src.MovieId
      }))
      .ForMember(dest => dest.Showtimes, opt => opt.MapFrom(src => src.Showtimes));
    CreateMap<Plan, PlanDto>()
      .ForMember(dest => dest.Listings, opt => opt.MapFrom(src => src.Listings));
  }
}
