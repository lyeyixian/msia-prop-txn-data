using MsiaPropertyTransaction.Application.Services;
using Xunit;

namespace MsiaPropertyTransaction.Tests;

public class S3PathValidationTests
{
    [Theory]
    [InlineData(null, "file.csv", "bucketName")]
    [InlineData("", "file.csv", "bucketName")]
    [InlineData("   ", "file.csv", "bucketName")]
    [InlineData("bucket", null, "fileKey")]
    [InlineData("bucket", "", "fileKey")]
    [InlineData("bucket", "   ", "fileKey")]
    public void ValidateS3Path_InvalidPaths_ThrowsArgumentException(string? bucketName, string? fileKey, string paramName)
    {
        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() =>
            S3PathValidator.ValidateS3Path(bucketName!, fileKey!));
        Assert.Equal(paramName, ex.ParamName);
    }

    [Theory]
    [InlineData("ab", "file.csv")]  // Too short
    [InlineData("this-bucket-name-is-way-too-long-and-exceeds-the-maximum-length-allowed-by-s3", "file.csv")]  // Too long
    public void ValidateS3Path_InvalidBucketNameLength_ThrowsArgumentException(string bucketName, string fileKey)
    {
        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() =>
            S3PathValidator.ValidateS3Path(bucketName, fileKey));
        Assert.Equal("bucketName", ex.ParamName);
    }

    [Theory]
    [InlineData("Bucket-Name-With-Capitals", "file.csv")]
    [InlineData("bucket_name_underscores", "file.csv")]
    [InlineData("bucket@name#special", "file.csv")]
    public void ValidateS3Path_InvalidBucketNameChars_ThrowsArgumentException(string bucketName, string fileKey)
    {
        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() =>
            S3PathValidator.ValidateS3Path(bucketName, fileKey));
        Assert.Equal("bucketName", ex.ParamName);
    }

    [Theory]
    [InlineData("my-bucket", "file.csv")]
    [InlineData("my.bucket", "path/to/file.csv")]
    [InlineData("bucket123", "data.csv")]
    [InlineData("a-bucket-name-that-is-exactly-63-characters-long-for-testing", "file.csv")]
    public void ValidateS3Path_ValidPaths_DoesNotThrow(string bucketName, string fileKey)
    {
        // Act & Assert
        var exception = Record.Exception(() => 
            S3PathValidator.ValidateS3Path(bucketName, fileKey));
        Assert.Null(exception);
    }
}
