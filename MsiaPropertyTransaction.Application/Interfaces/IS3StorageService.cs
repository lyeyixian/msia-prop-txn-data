namespace MsiaPropertyTransaction.Application.Interfaces;

public interface IS3StorageService
{
    Task<Stream> GetFileStreamAsync(string bucketName, string fileKey, CancellationToken cancellationToken = default);
    Task<bool> FileExistsAsync(string bucketName, string fileKey, CancellationToken cancellationToken = default);
}
