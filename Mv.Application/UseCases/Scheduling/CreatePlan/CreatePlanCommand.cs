using FluentValidation;
using Mv.Application.Abstractions;

namespace Mv.Application.UseCases.Scheduling.CreatePlan;

public record CreatePlanCommand(string? Name, DateOnly StartDate, DateOnly EndDate) : ICommand<Guid>;

public class CreatePlanValidator : AbstractValidator<CreatePlanCommand> {
  public CreatePlanValidator() {
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
