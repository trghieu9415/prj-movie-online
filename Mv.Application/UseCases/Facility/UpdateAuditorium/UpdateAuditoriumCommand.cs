using FluentValidation;
using Mv.Application.Abstractions;

namespace Mv.Application.UseCases.Facility.UpdateAuditorium;

public record UpdateAuditoriumCommand(Guid Id, string Name) : ICommand<bool>;

public class UpdateAuditoriumValidator : AbstractValidator<UpdateAuditoriumCommand> {
  public UpdateAuditoriumValidator() {
    RuleFor(x => x.Id)
      .NotEmpty().WithMessage("Id không hợp lệ.");

    RuleFor(x => x.Name)
      .NotEmpty().WithMessage("Tên phòng chiếu không được để trống.")
      .MaximumLength(100).WithMessage("Tên phòng chiếu không được vượt quá 100 ký tự.");
  }
}
