using MsiaPropertyTransaction.Application.Settings;

namespace MsiaPropertyTransaction.Infrastructure.Validation;

public static class S3ConfigurationValidator
{
    public static void Validate(S3StorageSettings settings)
    {
        if (string.IsNullOrWhiteSpace(settings.ServiceUrl))
        {
            throw new InvalidOperationException("S3 Storage configuration error: ServiceUrl is required");
        }

        if (string.IsNullOrWhiteSpace(settings.Region))
        {
            throw new InvalidOperationException("S3 Storage configuration error: Region is required");
        }

        if (string.IsNullOrWhiteSpace(settings.AccessKey))
        {
            throw new InvalidOperationException("S3 Storage configuration error: AccessKey is required");
        }

        if (string.IsNullOrWhiteSpace(settings.SecretKey))
        {
            throw new InvalidOperationException("S3 Storage configuration error: SecretKey is required");
        }

        if (string.IsNullOrWhiteSpace(settings.BucketName))
        {
            throw new InvalidOperationException("S3 Storage configuration error: BucketName is required");
        }

        // Validate bucket name format
        if (settings.BucketName.Length < 3 || settings.BucketName.Length > 63)
        {
            throw new InvalidOperationException("S3 Storage configuration error: BucketName must be between 3 and 63 characters");
        }

        if (!System.Text.RegularExpressions.Regex.IsMatch(settings.BucketName, "^[a-z0-9.-]+$"))
        {
            throw new InvalidOperationException("S3 Storage configuration error: BucketName can only contain lowercase letters, numbers, hyphens, and periods");
        }
    }
}
