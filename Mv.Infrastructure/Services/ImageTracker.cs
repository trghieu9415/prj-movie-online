using Microsoft.EntityFrameworkCore;
using Mv.Domain.Entities;
using Mv.Infrastructure.Persistence;
using Mv.Infrastructure.Persistence.Identity;
using Mv.Infrastructure.Services.Abstractions;

namespace Mv.Infrastructure.Services;

public class ImageTracker(AppDbContext dbContext) : IImageTracker {
  public async Task<HashSet<string>> GetInUseImageUrlsAsync(CancellationToken ct = default) {
    var movieUrls = await dbContext.Set<Movie>()
      .Where(x => !x.IsDeleted && !string.IsNullOrEmpty(x.PosterUrl))
      .Select(x => x.PosterUrl)
      .ToListAsync(ct);

    var userUrls = await dbContext.Set<AppUser>()
      .Where(x => !x.IsDeleted && !string.IsNullOrEmpty(x.Url))
      .Select(x => x.Url!)
      .ToListAsync(ct);

    var allUrls = movieUrls.Concat(userUrls);
    return [..allUrls];
  }
}
