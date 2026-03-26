using Mv.Application.Exceptions;
using Mv.Application.Ports.Logging;
using Mv.Domain.Exceptions;
using Mv.Infrastructure.Exceptions;
using Mv.Presentation.Response;

namespace Mv.Presentation.Middlewares;

public class GlobalExceptionMiddleware(
  RequestDelegate next,
  IAppLogger<GlobalExceptionMiddleware> logger
) {
  public async Task InvokeAsync(HttpContext context) {
    try {
      await next(context);
    } catch (Exception ex) {
      var shouldLogToFile = ex
        is DomainException
        or WorkflowException
        or InvalidInputException
        or InfrastructureException;

      if (shouldLogToFile) {
        logger.LogBusinessError(ex, "{Message}", ex.Message);
      } else {
        logger.LogSystemError(ex, "{Message}", ex.Message);
      }


      var (statusCode, responseModel) = MapException(ex);
      context.Response.ContentType = "application/json";
      context.Response.StatusCode = statusCode;
      await context.Response.WriteAsJsonAsync(responseModel);
    }
  }

  private static (int StatusCode, object ResponseValue) MapException(Exception ex) {
    return ex switch {
      InvalidInputException iIEx => (
        422,
        AppResponse.Fail(
          iIEx.Errors,
          iIEx.Errors.FirstOrDefault() ?? "Dữ liệu không hợp lệ", 422).Value!
      ),
      DomainException dEx => (400, AppResponse.Fail(dEx.Message, 400).Value!),
      InfrastructureException iEx => (500, AppResponse.Fail(iEx.Message, 500).Value!),
      WorkflowException wfEx => (wfEx.StatusCode, AppResponse.Fail(wfEx.Message, wfEx.StatusCode).Value!),
      _ => (500, AppResponse.Fail("Lỗi hệ thống không xác định", 500).Value!)
    };
  }
}
