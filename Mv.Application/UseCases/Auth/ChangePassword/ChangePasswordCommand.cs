using Mv.Application.Abstractions;

namespace Mv.Application.UseCases.Auth.ChangePassword;

public record ChangePasswordCommand(string OldPassword, string NewPassword) : ICommand<bool>;
