using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Mv.Application.DTOs;
using Mv.Application.Repositories.Read;
using Mv.Domain.Entities;

namespace Mv.Infrastructure.Persistence.Repositories.Read;

public class ListingReadRepository(AppDbContext dbContext, IMapper mapper)
  : EfReadRepository<Listing, ListingDto>(dbContext, mapper), IListingReadRepository {
  public async Task<ListingDto?> GetListingDetailsAsync(Guid id, CancellationToken ct = default) {
    var movies = DbContext.Set<Movie>().AsNoTracking();

    var listingDto = await DbSet.AsNoTracking()
      .Where(x => x.Id == id && !x.IsDeleted)
      .Select(l => new ListingDto {
        Id = l.Id,
        Movie = movies.Where(m => m.Id == l.MovieId && !m.IsDeleted)
          .Select(m => new MovieDto {
            Id = m.Id,
            Name = m.Name,
            Duration = m.Duration,
            PosterUrl = m.PosterUrl
          })
          .FirstOrDefault(),
        Showtimes = l.Showtimes.Where(s => !s.IsDeleted).Select(s => new ShowtimeDto {
          Id = s.Id,
          AuditoriumId = s.AuditoriumId,
          Date = s.Date,
          StartAt = s.StartAt,
          EndAt = s.EndAt
        }).ToList()
      }).FirstOrDefaultAsync(ct);

    return listingDto;
  }
}
