namespace Mv.Application.Constants;

public static class ClientMethods {
  // --- System ---
  public const string ForceLogout = "force-logout";
  public const string ReceiveNotification = "receive-notification";

  // --- Business ---
  public const string PaymentRefund = "payment-refund";
  public const string PaymentSuccess = "payment-success";
  public const string PaymentFailed = "payment-failed";
  public const string OrderCompleted = "payment-completed";
  public const string SeatLocked = "seat-locked";
  public const string SeatReleased = "seat-released";
}
