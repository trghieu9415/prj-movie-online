using Mv.Application.DTOs.Base;

namespace Mv.Application.DTOs;

public record MovieDto(Guid Id, string Name, int Duration, string PosterUrl) : IdDto(Id);
