using FluentValidation;
using Mv.Application.Abstractions;

namespace Mv.Application.UseCases.Facility.AddAuditorium;

public record AddAuditoriumCommand(string Name, int RowCount, int SeatsPerRow) : ICommand<Guid>;

public class AddAuditoriumValidator : AbstractValidator<AddAuditoriumCommand> {
  public AddAuditoriumValidator() {
    RuleFor(x => x.Name)
      .NotEmpty().WithMessage("Tên phòng chiếu không được để trống.")
      .MaximumLength(100).WithMessage("Tên phòng chiếu không được vượt quá 100 ký tự.");

    RuleFor(x => x.RowCount)
      .GreaterThan(0).WithMessage("Số hàng phải lớn hơn 0.")
      .LessThanOrEqualTo(50).WithMessage("Số hàng không được vượt quá 50.");

    RuleFor(x => x.SeatsPerRow)
      .GreaterThan(0).WithMessage("Số ghế mỗi hàng phải lớn hơn 0.")
      .LessThanOrEqualTo(50).WithMessage("Số ghế mỗi hàng không được vượt quá 50.");
  }
}
