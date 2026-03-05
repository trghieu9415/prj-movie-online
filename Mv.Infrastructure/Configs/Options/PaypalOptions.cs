namespace Mv.Infrastructure.Configs.Options;

public class PayPalOptions : IOptionSection {
  public string ClientId { get; set; } = string.Empty;
  public string ClientSecret { get; set; } = string.Empty;
  public string Mode { get; set; } = "sandbox";

  public string SuccessUrl { get; set; } = string.Empty;
  public string CancelUrl { get; set; } = string.Empty;
  public static string SectionName => "PayPal";
}
