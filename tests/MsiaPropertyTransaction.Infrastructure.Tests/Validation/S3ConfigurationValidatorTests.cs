using MsiaPropertyTransaction.Application.Settings;
using MsiaPropertyTransaction.Infrastructure.Validation;
using Xunit;

namespace MsiaPropertyTransaction.Infrastructure.Tests;

public class S3ConfigurationValidatorTests
{
    [Theory]
    [InlineData(null, "us-east-1", "access", "secret", "bucket")]
    [InlineData("", "us-east-1", "access", "secret", "bucket")]
    public void Validate_MissingServiceUrl_ThrowsInvalidOperationException(string? serviceUrl, string region, string accessKey, string secretKey, string bucketName)
    {
        // Arrange
        var settings = new S3StorageSettings
        {
            ServiceUrl = serviceUrl!,
            Region = region,
            AccessKey = accessKey,
            SecretKey = secretKey,
            BucketName = bucketName
        };

        // Act & Assert
        var ex = Assert.Throws<InvalidOperationException>(() => S3ConfigurationValidator.Validate(settings));
        Assert.Contains("ServiceUrl", ex.Message);
    }

    [Theory]
    [InlineData("http://localhost:4566", null, "access", "secret", "bucket")]
    [InlineData("http://localhost:4566", "", "access", "secret", "bucket")]
    public void Validate_MissingRegion_ThrowsInvalidOperationException(string serviceUrl, string? region, string accessKey, string secretKey, string bucketName)
    {
        // Arrange
        var settings = new S3StorageSettings
        {
            ServiceUrl = serviceUrl,
            Region = region!,
            AccessKey = accessKey,
            SecretKey = secretKey,
            BucketName = bucketName
        };

        // Act & Assert
        var ex = Assert.Throws<InvalidOperationException>(() => S3ConfigurationValidator.Validate(settings));
        Assert.Contains("Region", ex.Message);
    }

    [Theory]
    [InlineData("http://localhost:4566", "us-east-1", null, "secret", "bucket")]
    [InlineData("http://localhost:4566", "us-east-1", "", "secret", "bucket")]
    public void Validate_MissingAccessKey_ThrowsInvalidOperationException(string serviceUrl, string region, string? accessKey, string secretKey, string bucketName)
    {
        // Arrange
        var settings = new S3StorageSettings
        {
            ServiceUrl = serviceUrl,
            Region = region,
            AccessKey = accessKey!,
            SecretKey = secretKey,
            BucketName = bucketName
        };

        // Act & Assert
        var ex = Assert.Throws<InvalidOperationException>(() => S3ConfigurationValidator.Validate(settings));
        Assert.Contains("AccessKey", ex.Message);
    }

    [Theory]
    [InlineData("http://localhost:4566", "us-east-1", "access", null, "bucket")]
    [InlineData("http://localhost:4566", "us-east-1", "access", "", "bucket")]
    public void Validate_MissingSecretKey_ThrowsInvalidOperationException(string serviceUrl, string region, string accessKey, string? secretKey, string bucketName)
    {
        // Arrange
        var settings = new S3StorageSettings
        {
            ServiceUrl = serviceUrl,
            Region = region,
            AccessKey = accessKey,
            SecretKey = secretKey!,
            BucketName = bucketName
        };

        // Act & Assert
        var ex = Assert.Throws<InvalidOperationException>(() => S3ConfigurationValidator.Validate(settings));
        Assert.Contains("SecretKey", ex.Message);
    }

    [Theory]
    [InlineData("http://localhost:4566", "us-east-1", "access", "secret", null)]
    [InlineData("http://localhost:4566", "us-east-1", "access", "secret", "")]
    public void Validate_MissingBucketName_ThrowsInvalidOperationException(string serviceUrl, string region, string accessKey, string secretKey, string? bucketName)
    {
        // Arrange
        var settings = new S3StorageSettings
        {
            ServiceUrl = serviceUrl,
            Region = region,
            AccessKey = accessKey,
            SecretKey = secretKey,
            BucketName = bucketName!
        };

        // Act & Assert
        var ex = Assert.Throws<InvalidOperationException>(() => S3ConfigurationValidator.Validate(settings));
        Assert.Contains("BucketName", ex.Message);
    }

    [Theory]
    [InlineData("ab")]  // Too short
    [InlineData("this-bucket-name-is-way-too-long-and-exceeds-the-maximum-length-allowed")]  // Too long
    public void Validate_InvalidBucketNameLength_ThrowsInvalidOperationException(string bucketName)
    {
        // Arrange
        var settings = new S3StorageSettings
        {
            ServiceUrl = "http://localhost:4566",
            Region = "us-east-1",
            AccessKey = "access",
            SecretKey = "secret",
            BucketName = bucketName
        };

        // Act & Assert
        var ex = Assert.Throws<InvalidOperationException>(() => S3ConfigurationValidator.Validate(settings));
        Assert.Contains("BucketName", ex.Message);
    }

    [Theory]
    [InlineData("Bucket-Name-With-Capitals")]
    [InlineData("bucket_name_underscores")]
    [InlineData("bucket@name#special")]
    public void Validate_InvalidBucketNameChars_ThrowsInvalidOperationException(string bucketName)
    {
        // Arrange
        var settings = new S3StorageSettings
        {
            ServiceUrl = "http://localhost:4566",
            Region = "us-east-1",
            AccessKey = "access",
            SecretKey = "secret",
            BucketName = bucketName
        };

        // Act & Assert
        var ex = Assert.Throws<InvalidOperationException>(() => S3ConfigurationValidator.Validate(settings));
        Assert.Contains("BucketName", ex.Message);
    }

    [Fact]
    public void Validate_ValidConfiguration_DoesNotThrow()
    {
        // Arrange
        var settings = new S3StorageSettings
        {
            ServiceUrl = "http://localhost:4566",
            Region = "us-east-1",
            AccessKey = "test",
            SecretKey = "test",
            BucketName = "my-bucket"
        };

        // Act & Assert
        var exception = Record.Exception(() => S3ConfigurationValidator.Validate(settings));
        Assert.Null(exception);
    }
}
