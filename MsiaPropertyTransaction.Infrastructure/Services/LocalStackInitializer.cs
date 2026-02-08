using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MsiaPropertyTransaction.Application.Settings;

namespace MsiaPropertyTransaction.Infrastructure.Services;

public class LocalStackInitializer
{
    private readonly S3StorageSettings _settings;
    private readonly ILogger<LocalStackInitializer> _logger;

    public LocalStackInitializer(IOptions<S3StorageSettings> settings, ILogger<LocalStackInitializer> logger)
    {
        _settings = settings.Value;
        _logger = logger;
    }

    public async Task InitializeAsync()
    {
        try
        {
            _logger.LogInformation("Initializing LocalStack S3 bucket...");

            var config = new AmazonS3Config
            {
                ServiceURL = _settings.ServiceUrl,
                ForcePathStyle = true
            };

            var credentials = new Amazon.Runtime.BasicAWSCredentials(_settings.AccessKey, _settings.SecretKey);
            using var client = new AmazonS3Client(credentials, config);

            // Check if bucket exists
            var buckets = await client.ListBucketsAsync();
            if (buckets.Buckets.Any(b => b.BucketName == _settings.BucketName))
            {
                _logger.LogInformation("Bucket '{BucketName}' already exists", _settings.BucketName);
                return;
            }

            // Create bucket
            var putBucketRequest = new PutBucketRequest
            {
                BucketName = _settings.BucketName
            };

            await client.PutBucketAsync(putBucketRequest);
            _logger.LogInformation("Created S3 bucket: {BucketName}", _settings.BucketName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to initialize LocalStack S3 bucket");
            // Don't throw - allow app to start even if initialization fails
        }
    }
}
