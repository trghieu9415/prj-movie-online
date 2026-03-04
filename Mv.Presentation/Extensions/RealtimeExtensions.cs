using Mv.Application.Ports.Realtime;
using Mv.Presentation.Adapters.Realtime;

namespace Mv.Presentation.Extensions;

public static class RealtimeExtensions {
  public static IServiceCollection AddSignalRAdapters(this IServiceCollection services) {
    services.AddSignalR();
    services.AddScoped<IShowtimeNotifier, ShowtimeNotifier>();
    services.AddScoped<IUserNotifier, UserNotifier>();

    return services;
  }
}
