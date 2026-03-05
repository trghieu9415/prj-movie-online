using System.Globalization;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using Domain.Entities;
using Mv.Application.Ports.Gateway;
using Mv.Infrastructure.Configs.Options;

namespace Mv.Infrastructure.Adapters.Gateway.Transaction;

public class PaypalGateway : IPaymentGateway {
  private readonly HttpClient _client;
  private readonly PayPalOptions _options;

  public PaypalGateway(PayPalOptions options, IHttpClientFactory clientFactory) {
    _options = options;
    _client = clientFactory.CreateClient();

    var baseUrl =
      options.Mode.Equals("live", StringComparison.CurrentCultureIgnoreCase)
        ? "https://api-m.paypal.com"
        : "https://api-m.sandbox.paypal.com";

    _client.BaseAddress = new Uri(baseUrl);
  }

  public string CreatePaymentUrl(Payment payment, Order order) {
    var token = GetAccessToken();
    if (string.IsNullOrEmpty(token)) {
      return string.Empty;
    }

    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

    var orderRequest = new {
      intent = "CAPTURE",
      purchase_units = new[] {
        new {
          reference_id = payment.Id.ToString(),
          amount = new {
            currency_code = "USD",
            value = payment.Amount.ToString("0.00", CultureInfo.InvariantCulture)
          },
          description = $"Thanh toán vé phim {order.Movie.Name}"
        }
      },
      application_context = new {
        return_url = _options.SuccessUrl,
        cancel_url = _options.CancelUrl,
        user_action = "PAY_NOW"
      }
    };

    var content = new StringContent(JsonSerializer.Serialize(orderRequest), Encoding.UTF8, "application/json");
    var response = _client.PostAsync("/v2/checkout/orders", content).GetAwaiter().GetResult();

    if (!response.IsSuccessStatusCode) {
      return string.Empty;
    }

    var json = JsonNode.Parse(response.Content.ReadAsStringAsync().GetAwaiter().GetResult());
    var approveLink = json?["links"]?.AsArray().FirstOrDefault(x => x?["rel"]?.ToString() == "approve")?["href"]
      ?.ToString();

    return approveLink ?? string.Empty;
  }

  public (bool isSucceed, string transactionId) VerifyPayment(GatewayPayload payload) {
    if (payload is not PaypalGatewayPayload paypalPayload || string.IsNullOrEmpty(paypalPayload.OrderId)) {
      return (false, string.Empty);
    }

    try {
      var token = GetAccessToken();
      _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

      var content = new StringContent("", Encoding.UTF8, "application/json");
      var response = _client.PostAsync($"/v2/checkout/orders/{paypalPayload.OrderId}/capture", content)
        .GetAwaiter()
        .GetResult();

      if (!response.IsSuccessStatusCode) {
        return (false, string.Empty);
      }

      var jsonBody = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
      var json = JsonNode.Parse(jsonBody);

      var status = json?["status"]?.ToString();
      var captureId = json?["purchase_units"]?[0]?["payments"]?["captures"]?[0]?["id"]?.ToString();

      if (status == "COMPLETED" && !string.IsNullOrEmpty(captureId)) {
        return (true, captureId);
      }

      return (false, string.Empty);
    } catch {
      return (false, string.Empty);
    }
  }

  public bool RefundPayment(Payment payment) {
    try {
      var token = GetAccessToken();
      if (string.IsNullOrEmpty(token)) {
        return false;
      }

      _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

      var refundRequest = new {
        note_to_payer = "Hoàn tiền vé phim"
      };

      var content = new StringContent(JsonSerializer.Serialize(refundRequest), Encoding.UTF8, "application/json");

      var response = _client
        .PostAsync($"/v2/payments/captures/{payment.TransactionId}/refund", content)
        .GetAwaiter()
        .GetResult();

      if (!response.IsSuccessStatusCode) {
        return false;
      }

      var json = JsonNode.Parse(response.Content.ReadAsStringAsync().GetAwaiter().GetResult());
      var status = json?["status"]?.ToString();

      return status == "COMPLETED";
    } catch {
      return false;
    }
  }

  public GatewayPayload ToGatewayPayload(object data) {
    return new PaypalGatewayPayload(data.ToString() ?? string.Empty);
  }

  private string GetAccessToken() {
    var authBytes = Encoding.ASCII.GetBytes($"{_options.ClientId}:{_options.ClientSecret}");
    var request = new HttpRequestMessage(HttpMethod.Post, "/v1/oauth2/token");
    request.Headers.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authBytes));
    request.Content =
      new StringContent("grant_type=client_credentials", Encoding.UTF8, "application/x-www-form-urlencoded");

    var response = _client.SendAsync(request).GetAwaiter().GetResult();
    if (!response.IsSuccessStatusCode) {
      return string.Empty;
    }

    var json = JsonNode.Parse(response.Content.ReadAsStringAsync().GetAwaiter().GetResult());
    return json?["access_token"]?.ToString() ?? string.Empty;
  }
}

public record PaypalGatewayPayload(string OrderId) : GatewayPayload;
