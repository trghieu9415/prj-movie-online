using Quartz;

namespace Mv.Worker;

public static class JobRegistration {
  public static void RegisterApplicationJobs(this IServiceCollectionQuartzConfigurator q) {}

  // NOTE: ========== [Helper Methods] ==========
  private static TimeZoneInfo GetVnTimeZone() {
    var timeZoneId = Environment.OSVersion.Platform == PlatformID.Win32NT
      ? "SE Asia Standard Time"
      : "Asia/Ho_Chi_Minh";
    return TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
  }

  private static void ScheduleJob<TJob>(this IServiceCollectionQuartzConfigurator q, string cronExpression,
    string description = "")
    where TJob : IJob {
    var jobKey = new JobKey(typeof(TJob).Name);
    q.AddJob<TJob>(opts => opts.WithIdentity(jobKey).WithDescription(description));

    q.AddTrigger(opts => opts
      .ForJob(jobKey)
      .WithIdentity($"{typeof(TJob).Name}-Trigger")
      .WithCronSchedule(cronExpression, x => x.InTimeZone(GetVnTimeZone()))
    );
  }
}
