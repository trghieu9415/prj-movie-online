using FluentValidation;
using Mv.Application.Abstractions;

namespace Mv.Application.UseCases.Facility.AddAuditorium;

public record AddAuditoriumCommand(string Name, int RowCount, int SeatsPerRow) : ICommand<Guid>;

public class AddAuditoriumValidator : AbstractValidator<AddAuditoriumCommand> {
  public AddAuditoriumValidator() {
    RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
    RuleFor(x => x.RowCount).GreaterThan(0).WithMessage("Số hàng ghế phải lớn hơn 0");
    RuleFor(x => x.SeatsPerRow).GreaterThan(0).WithMessage("Số ghế mỗi hàng phải lớn hơn 0");
  }
}
