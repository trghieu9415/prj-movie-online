using Microsoft.Extensions.Logging;
using Mv.Application.Ports.Logging;

namespace Mv.Infrastructure.Adapters.Logging;

public class LoggerAdapter<T>(ILogger<T> logger) : IAppLogger<T> {
  public void LogBusinessInformation(string message, params object[] args) {
    using (logger.BeginScope(new Dictionary<string, object> { { "LogType", "BusinessInfo" } })) {
      logger.LogInformation(message, args);
    }
  }

  public void LogBusinessError(Exception ex, string message, params object[] args) {
    using (logger.BeginScope(new Dictionary<string, object> { { "LogType", "BusinessError" } })) {
      logger.LogWarning(ex, message, args);
    }
  }

  public void LogSystemWarning(string message, params object[] args) {
    using (logger.BeginScope(new Dictionary<string, object> { { "LogType", "SystemWarning" } })) {
      logger.LogWarning(message, args);
    }
  }

  public void LogSystemError(Exception ex, string message, params object[] args) {
    logger.LogError(ex, message, args);
  }
}
