using FluentValidation;
using Mv.Application.Abstractions;
using Mv.Application.DTOs;

namespace Mv.Application.UseCases.Scheduling.GetListing;

public record GetListingQuery(Guid Id) : IQuery<GetListingResult>;

public record GetListingResult(ListingDto Listing);

public class GetListingValidator : AbstractValidator<GetListingQuery> {
  public GetListingValidator() {
    RuleFor(x => x.Id)
      .NotEmpty().WithMessage("Id không hợp lệ.");
  }
}
