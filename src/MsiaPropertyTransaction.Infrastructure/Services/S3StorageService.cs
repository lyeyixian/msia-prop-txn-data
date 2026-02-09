using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using MsiaPropertyTransaction.Application.Interfaces;

namespace MsiaPropertyTransaction.Infrastructure.Services;

public class S3StorageService : IS3StorageService
{
    private readonly IAmazonS3 _s3Client;

    public S3StorageService(string serviceUrl, string region, string accessKey, string secretKey)
    {
        var config = new AmazonS3Config
        {
            ServiceURL = serviceUrl,
            RegionEndpoint = RegionEndpoint.GetBySystemName(region),
            ForcePathStyle = true // Required for LocalStack and Railway Storage
        };

        var credentials = new BasicAWSCredentials(accessKey, secretKey);
        _s3Client = new AmazonS3Client(credentials, config);
    }

    // Constructor for testing with mock client
    public S3StorageService(IAmazonS3 s3Client)
    {
        _s3Client = s3Client ?? throw new ArgumentNullException(nameof(s3Client));
    }

    public async Task<Stream> GetFileStreamAsync(string bucketName, string fileKey, CancellationToken cancellationToken = default)
    {
        try
        {
            var request = new GetObjectRequest
            {
                BucketName = bucketName,
                Key = fileKey
            };

            var response = await _s3Client.GetObjectAsync(request, cancellationToken);
            return response.ResponseStream;
        }
        catch (AmazonS3Exception ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            throw new FileNotFoundException($"File '{fileKey}' not found in bucket '{bucketName}'", fileKey, ex);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to retrieve file '{fileKey}' from S3: {ex.Message}", ex);
        }
    }

    public async Task<bool> FileExistsAsync(string bucketName, string fileKey, CancellationToken cancellationToken = default)
    {
        try
        {
            var request = new GetObjectMetadataRequest
            {
                BucketName = bucketName,
                Key = fileKey
            };

            await _s3Client.GetObjectMetadataAsync(request, cancellationToken);
            return true;
        }
        catch (AmazonS3Exception ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return false;
        }
        catch (Exception)
        {
            return false;
        }
    }
}
