using Domain.Entities;
using Mv.Application.DTOs;

namespace Mv.Application.Repositories.Read;

public interface IListingReadRepository : IReadRepository<Listing, ListingDto> {
  Task<ListingDto?> GetListingDetailsAsync(Guid id, CancellationToken ct = default);
}
