namespace Mv.Presentation.Controllers.Forms;

public record ImageForm {
  public IFormFile? Image { get; init; }
}

public record ImageFormWithType : ImageForm {
  public ImageType Type { get; init; }
}

public enum ImageType {
  UserProfile,
  MoviePoster
}
