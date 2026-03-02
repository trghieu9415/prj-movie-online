using AutoMapper;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Mv.Application.DTOs;
using Mv.Application.Ports.Repositories.Read;
using Mv.Infrastructure.Persistence;

namespace Mv.Infrastructure.Adapters.Repositories.Read;

public class ListingReadRepository(AppDbContext dbContext, IMapper mapper)
  : EfReadRepository<Listing, ListingDto>(dbContext, mapper), IListingReadRepository {
  public async Task<ListingDto?> GetListingDetailsAsync(Guid id, CancellationToken ct = default) {
    var movies = DbContext.Set<Movie>().AsNoTracking();

    var listingDto = await DbSet.AsNoTracking()
      .Where(x => x.Id == id && !x.IsDeleted)
      .Select(l => new ListingDto(
        l.Id, movies.Where(m => m.Id == l.MovieId && !m.IsDeleted)
          .Select(m => new MovieDto(m.Id, m.Name, m.Duration, m.PosterUrl))
          .FirstOrDefault(),
        l.Showtimes.Where(s => !s.IsDeleted).Select(s => new ShowtimeDto(
          s.Id, s.AuditoriumId, s.DayOfWeek, s.StartAt, s.EndAt
        )).ToList()
      ))
      .FirstOrDefaultAsync(ct);

    return listingDto;
  }
}
