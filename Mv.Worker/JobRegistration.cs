using Mv.Worker.BackgroundJobs;
using Quartz;

namespace Mv.Worker;

public static class JobRegistration {
  public static void RegisterApplicationJobs(this IServiceCollectionQuartzConfigurator q) {
    q.ScheduleCronJob<FallbackOrderCleanupJob>("0 0/45 * * * ?",
      "[Dự phòng] Quét và hủy các đơn hàng bị kẹt Pending quá hạn do lỗi hệ thống"
    );
    q.ScheduleCronJob<CleanupOrphanedImagesJob>("0 0 2 ? * SUN",
      "Dọn dẹp ảnh rác trên S3 lúc 2h sáng Chủ Nhật hàng tuần"
    );
  }

  // NOTE: ========== [Helper Methods] ==========
  private static TimeZoneInfo GetVnTimeZone() {
    var timeZoneId = Environment.OSVersion.Platform == PlatformID.Win32NT
      ? "SE Asia Standard Time"
      : "Asia/Ho_Chi_Minh";
    return TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
  }

  private static void ScheduleCronJob<TJob>(
    this IServiceCollectionQuartzConfigurator q,
    string cronExpression,
    string description = ""
  ) where TJob : IJob {
    var jobKey = new JobKey(typeof(TJob).Name);
    q.AddJob<TJob>(opts => opts.WithIdentity(jobKey).WithDescription(description));

    q.AddTrigger(opts => opts
      .ForJob(jobKey)
      .WithIdentity($"{typeof(TJob).Name}-Trigger")
      .WithCronSchedule(cronExpression, x => x.InTimeZone(GetVnTimeZone()))
    );
  }

  private static void ScheduleStartupJob<TJob>(
    this IServiceCollectionQuartzConfigurator q,
    TimeSpan delay,
    string description = ""
  ) where TJob : IJob {
    var jobKey = new JobKey(typeof(TJob).Name);
    q.AddJob<TJob>(opts => opts.WithIdentity(jobKey).WithDescription(description));
    q.AddTrigger(opts => opts
      .ForJob(jobKey)
      .WithIdentity($"{typeof(TJob).Name}-Trigger")
      .StartAt(DateBuilder.EvenSecondDate(DateTimeOffset.Now.Add(delay)))
    );
  }

  private static void SchedulePeriodicJob<TJob>(
    this IServiceCollectionQuartzConfigurator q,
    TimeSpan interval,
    string description = ""
  ) where TJob : IJob {
    var jobKey = new JobKey(typeof(TJob).Name);
    q.AddJob<TJob>(opts => opts.WithIdentity(jobKey).WithDescription(description));

    q.AddTrigger(opts => opts
      .ForJob(jobKey)
      .WithIdentity($"{typeof(TJob).Name}-Trigger")
      .StartNow()
      .WithSimpleSchedule(x => x
        .WithInterval(interval)
        .RepeatForever()
      )
    );
  }
}
