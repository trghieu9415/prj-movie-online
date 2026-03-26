using Mv.Application.DTOs;
using Mv.Domain.Entities;

namespace Mv.Application.Repositories.Read;

public interface IListingReadRepository : IReadRepository<Listing, ListingDto> {
  Task<ListingDto?> GetListingDetailsAsync(Guid id, CancellationToken ct = default);
}
