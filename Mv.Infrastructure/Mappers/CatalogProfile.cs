using AutoMapper;
using Mv.Application.DTOs;
using Mv.Domain.Entities;

namespace Mv.Infrastructure.Mappers;

public class CatalogProfile : Profile {
  public CatalogProfile() {
    CreateMap<Movie, MovieDto>();
  }
}
