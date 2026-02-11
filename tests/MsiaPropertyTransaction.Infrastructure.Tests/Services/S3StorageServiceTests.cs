using Amazon.S3;
using Amazon.S3.Model;
using Moq;
using MsiaPropertyTransaction.Infrastructure.Services;
using System.Net;
using Xunit;

namespace MsiaPropertyTransaction.Infrastructure.Tests;

public class S3StorageServiceTests
{
    private readonly Mock<IAmazonS3> _mockS3Client;
    private readonly S3StorageService _service;

    public S3StorageServiceTests()
    {
        _mockS3Client = new Mock<IAmazonS3>();
        _service = new S3StorageService(_mockS3Client.Object);
    }

    [Fact]
    public async Task FileExistsAsync_FileExists_ReturnsTrue()
    {
        // Arrange
        var bucketName = "test-bucket";
        var fileKey = "test-file.csv";
        _mockS3Client
            .Setup(x => x.GetObjectMetadataAsync(It.Is<GetObjectMetadataRequest>(r => 
                r.BucketName == bucketName && r.Key == fileKey), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetObjectMetadataResponse());

        // Act
        var result = await _service.FileExistsAsync(bucketName, fileKey);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task FileExistsAsync_FileNotFound_ReturnsFalse()
    {
        // Arrange
        var bucketName = "test-bucket";
        var fileKey = "non-existent.csv";
        var exception = new AmazonS3Exception("Not found")
        {
            StatusCode = HttpStatusCode.NotFound
        };
        _mockS3Client
            .Setup(x => x.GetObjectMetadataAsync(It.IsAny<GetObjectMetadataRequest>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(exception);

        // Act
        var result = await _service.FileExistsAsync(bucketName, fileKey);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task FileExistsAsync_S3Error_ReturnsFalse()
    {
        // Arrange
        var bucketName = "test-bucket";
        var fileKey = "test-file.csv";
        _mockS3Client
            .Setup(x => x.GetObjectMetadataAsync(It.IsAny<GetObjectMetadataRequest>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Connection error"));

        // Act
        var result = await _service.FileExistsAsync(bucketName, fileKey);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task GetFileStreamAsync_FileExists_ReturnsStream()
    {
        // Arrange
        var bucketName = "test-bucket";
        var fileKey = "test-file.csv";
        var mockStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes("test content"));
        
        _mockS3Client
            .Setup(x => x.GetObjectAsync(It.Is<GetObjectRequest>(r => 
                r.BucketName == bucketName && r.Key == fileKey), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetObjectResponse 
            { 
                ResponseStream = mockStream 
            });

        // Act
        var result = await _service.GetFileStreamAsync(bucketName, fileKey);

        // Assert
        Assert.NotNull(result);
        Assert.IsAssignableFrom<Stream>(result);
    }

    [Fact]
    public async Task GetFileStreamAsync_FileNotFound_ThrowsFileNotFoundException()
    {
        // Arrange
        var bucketName = "test-bucket";
        var fileKey = "non-existent.csv";
        var exception = new AmazonS3Exception("Not found")
        {
            StatusCode = HttpStatusCode.NotFound
        };
        _mockS3Client
            .Setup(x => x.GetObjectAsync(It.IsAny<GetObjectRequest>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(exception);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<FileNotFoundException>(() => 
            _service.GetFileStreamAsync(bucketName, fileKey));
        Assert.Contains(fileKey, ex.Message);
        Assert.Contains(bucketName, ex.Message);
    }

    [Fact]
    public async Task GetFileStreamAsync_S3Error_ThrowsInvalidOperationException()
    {
        // Arrange
        var bucketName = "test-bucket";
        var fileKey = "test-file.csv";
        _mockS3Client
            .Setup(x => x.GetObjectAsync(It.IsAny<GetObjectRequest>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Connection failed"));

        // Act & Assert
        var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => 
            _service.GetFileStreamAsync(bucketName, fileKey));
        Assert.Contains("Failed to retrieve file", ex.Message);
    }

    [Fact]
    public void Constructor_WithNullClient_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new S3StorageService(null!));
    }
}
