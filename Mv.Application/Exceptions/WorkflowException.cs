namespace Mv.Application.Exceptions;

public class WorkflowException(string message, int statusCode = 400) : Exception(message) {
  public int StatusCode { get; } = statusCode;
}
