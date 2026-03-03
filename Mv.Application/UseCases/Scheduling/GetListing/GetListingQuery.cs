using Mv.Application.Abstractions;
using Mv.Application.DTOs;

namespace Mv.Application.UseCases.Scheduling.GetListing;

public record GetListingQuery(Guid Id) : IQuery<GetListingResult>;

public record GetListingResult(ListingDto Listing);
