namespace Mv.Application.Exceptions;

public class InvalidInputException(IEnumerable<string> errors) : Exception("Invalid data") {
  public IEnumerable<string> Errors { get; } = errors;
}
