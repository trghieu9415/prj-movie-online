using Mv.Application.Abstractions;
using Mv.Application.Models;

namespace Mv.Application.UseCases.Auth.Refresh;

public record RefreshCommand(string RefreshToken) : ICommand<RefreshResult>;

public record RefreshResult(AuthTokens AuthTokens);
