using FluentValidation;
using Mv.Application.Abstractions;

namespace Mv.Application.UseCases.Scheduling.UpdatePlan;

public record UpdatePlanCommand(Guid Id, string? Name, DateOnly StartDate, DateOnly EndDate) : ICommand<bool>;

public class UpdatePlanValidator : AbstractValidator<UpdatePlanCommand> {
  public UpdatePlanValidator() {
    RuleFor(x => x.Id)
      .NotEmpty().WithMessage("Id không hợp lệ.");

    RuleFor(x => x.Name)
      .NotEmpty().WithMessage("Tên kế hoạch không được để trống.")
      .MaximumLength(200).WithMessage("Tên kế hoạch không được vượt quá 200 ký tự.");

    RuleFor(x => x.StartDate)
      .NotEmpty().WithMessage("Ngày bắt đầu không được để trống.");

    RuleFor(x => x.EndDate)
      .NotEmpty().WithMessage("Ngày kết thúc không được để trống.")
      .GreaterThanOrEqualTo(x => x.StartDate)
      .WithMessage("Ngày kết thúc phải lớn hơn hoặc bằng ngày bắt đầu.");
  }
}
