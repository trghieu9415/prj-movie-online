using AutoMapper;
using Domain.Entities;
using Mv.Application.DTOs;

namespace Mv.Application.Mappers;

public class CatalogProfile : Profile {
  public CatalogProfile() {
    CreateMap<Movie, MovieDto>();
  }
}
