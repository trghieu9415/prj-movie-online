using Amazon.S3;
using Amazon.S3.Model;
using Mv.Infrastructure.Configs.Options;
using Mv.Infrastructure.Services.Abstractions;

namespace Mv.Infrastructure.Services;

public class S3StorageService : IStorageService {
  private readonly string _bucketName;
  private readonly IAmazonS3 _s3Client;

  public S3StorageService(S3Options s3Options) {
    var s3Config = new AmazonS3Config {
      ServiceURL = s3Options.ServiceUrl,
      ForcePathStyle = s3Options.ForcePathStyle
    };

    _bucketName = s3Options.BucketName;
    _s3Client = new AmazonS3Client(s3Options.AccessKey, s3Options.SecretKey, s3Config);
  }


  public async Task<string> UploadAsync(string fileName, Stream content, string ext, string folder,
    CancellationToken ct) {
    var key = string.IsNullOrEmpty(folder) ? $"{fileName}{ext}" : $"{folder}/{fileName}{ext}";

    var putRequest = new PutObjectRequest {
      BucketName = _bucketName,
      Key = key,
      InputStream = content,
      AutoCloseStream = true,
      ContentType = GetContentType(ext)
    };
    await _s3Client.PutObjectAsync(putRequest, ct);

    var publicUrl = $"{_s3Client.Config.ServiceURL.TrimEnd('/')}/{_bucketName}/{key}";
    return publicUrl;
  }

  public async Task DeleteAsync(string fileName, string folder, CancellationToken ct) {
    var key = string.IsNullOrEmpty(folder) ? fileName : $"{folder}/{fileName}";

    var deleteRequest = new DeleteObjectRequest {
      BucketName = _bucketName,
      Key = key
    };

    await _s3Client.DeleteObjectAsync(deleteRequest, ct);
  }

  // NOTE: ========== [Private Helper] ==========
  private string GetContentType(string ext) {
    return ext.ToLower() switch {
      ".jpg" or ".jpeg" => "image/jpeg",
      ".png" => "image/png",
      ".pdf" => "application/pdf",
      _ => "application/octet-stream"
    };
  }
}
