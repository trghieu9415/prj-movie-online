using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mv.Application.UseCases.Booking.CreatePayment;
using Mv.Application.UseCases.Booking.ProcessPayment;
using Mv.Presentation.Response;

namespace Mv.Presentation.Controllers.User;

[Authorize]
public class PaymentController : UserController {
  [HttpPost("create")]
  public async Task<IActionResult> CreatePayment([FromBody] CreatePaymentCommand command) {
    var paymentUrl = await Mediator.Send(command);
    return AppResponse.Success(paymentUrl);
  }

  [HttpPost("{id:guid}/verify")]
  public async Task<IActionResult> VerifyPayment(Guid id, [FromBody] ProcessPaymentCommand command) {
    var isSuccess = await Mediator.Send(command);
    return AppResponse.Success(isSuccess);
  }
}
