using Mv.Application.Abstractions;
using Mv.Application.Models;

namespace Mv.Application.UseCases.Auth.GetProfile;

public record GetProfileQuery : IQuery<GetProfileResult>;

public record GetProfileResult(User User);
