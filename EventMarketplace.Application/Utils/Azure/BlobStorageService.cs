using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;

namespace EventMarketplace.Application.Utils.Azure;

public class BlobStorageService(BlobServiceClient serviceClient) : IBlobStorageService
{
    public async Task<string> UploadFileAsync(IFormFile file, string containerName, CancellationToken cancellationToken = default)
    {
        var container = serviceClient.GetBlobContainerClient(containerName);
        await container.CreateIfNotExistsAsync(PublicAccessType.None, cancellationToken: cancellationToken);

        var fileName = Path.GetFileName(file.FileName);
        var uniqueName = $"{Guid.NewGuid():N}_{fileName}";

        var blobClient = container.GetBlobClient(uniqueName);
        var headers = new BlobHttpHeaders()
        {
            ContentType = file.ContentType
        };

        await using var stream = file.OpenReadStream();
        await blobClient.UploadAsync(stream, new BlobUploadOptions() { HttpHeaders = headers }, cancellationToken);

        return blobClient.Uri.ToString();
    }
}