namespace MsiaPropertyTransaction.Application.Services;

public static class S3PathValidator
{
    public static void ValidateS3Path(string bucketName, string fileKey)
    {
        if (string.IsNullOrWhiteSpace(bucketName))
        {
            throw new ArgumentException("Bucket name cannot be null or empty", nameof(bucketName));
        }

        if (string.IsNullOrWhiteSpace(fileKey))
        {
            throw new ArgumentException("File key cannot be null or empty", nameof(fileKey));
        }

        // Basic bucket name validation (S3 bucket naming rules)
        if (bucketName.Length < 3 || bucketName.Length > 63)
        {
            throw new ArgumentException("Bucket name must be between 3 and 63 characters", nameof(bucketName));
        }

        if (!System.Text.RegularExpressions.Regex.IsMatch(bucketName, "^[a-z0-9.-]+$"))
        {
            throw new ArgumentException("Bucket name can only contain lowercase letters, numbers, hyphens, and periods", nameof(bucketName));
        }
    }
}
