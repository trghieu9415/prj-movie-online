namespace Mv.Application.Constants;

public static class CacheTags {
  // NOTE: ========== [BUSINESS] ==========
  public static string CurrentPlan => "current-plan";

  public static string Auditorium(Guid id) {
    return $"auditorium:{id}";
  }

  public static string Listing(Guid id) {
    return $"listing:{id}";
  }

  // NOTE: ========== [SYSTEM] ==========
  public static string BlackList(string jti) {
    return $"blacklist:{jti}";
  }

  public static string UserStamp(Guid id) {
    return $"user:{id}:stamp";
  }
}
