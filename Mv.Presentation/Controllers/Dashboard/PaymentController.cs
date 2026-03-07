using Microsoft.AspNetCore.Mvc;
using Mv.Application.UseCases.System.RefundPayment;
using Mv.Presentation.Response;

namespace Mv.Presentation.Controllers.Dashboard;

public class PaymentController : DashboardController {
  [HttpPatch("{id:guid}/refund")]
  public async Task<IActionResult> Refund(Guid id) {
    var result = await Mediator.Send(new RefundPaymentCommand(id));
    return AppResponse.Success(result);
  }
}
