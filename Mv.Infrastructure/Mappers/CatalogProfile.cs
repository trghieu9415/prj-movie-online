using AutoMapper;
using Domain.Entities;
using Mv.Application.DTOs;

namespace Mv.Infrastructure.Mappers;

public class CatalogProfile : Profile {
  public CatalogProfile() {
    CreateMap<Movie, MovieDto>();
  }
}
