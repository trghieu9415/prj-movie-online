using Mv.Application.Ports.Logging;
using Mv.Infrastructure.Services.Abstractions;
using Quartz;

namespace Mv.Worker.BackgroundJobs;

public class CleanupOrphanedImagesJob(
  IStorageService storageService,
  IImageTracker imageTracker,
  IAppLogger<CleanupOrphanedImagesJob> logger
) : IJob {
  public async Task Execute(IJobExecutionContext context) {
    var ct = context.CancellationToken;
    var inUseUrls = await imageTracker.GetInUseImageUrlsAsync(ct);

    var s3Posters = await storageService.ListFilesAsync("posters", ct);
    var s3Profiles = await storageService.ListFilesAsync("profile", ct);
    var allS3Files = s3Posters.Concat(s3Profiles).ToList();

    foreach (var fileUrl in allS3Files.Where(fileUrl => !inUseUrls.Contains(fileUrl))) {
      try {
        var uri = new Uri(fileUrl);
        var segments = uri.AbsolutePath.TrimStart('/').Split('/');

        if (segments.Length < 3) {
          continue;
        }

        var folder = segments[1];
        var fileName = segments[2];

        await storageService.DeleteAsync(fileName, folder, ct);
      } catch (Exception ex) {
        logger.LogSystemError(ex, "Không thể xóa ảnh rác: {Url}", fileUrl);
      }
    }
  }
}
