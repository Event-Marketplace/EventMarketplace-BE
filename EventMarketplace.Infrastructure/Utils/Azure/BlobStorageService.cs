using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using EventMarketplace.Application.Exceptions;
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

    public async Task<string> UploadOrReplaceFileAsync(IFormFile file, string imageName, string containerName, CancellationToken cancellationToken)
    {
        var container = serviceClient.GetBlobContainerClient(containerName);
        var blobClient = container.GetBlobClient(imageName);
        
        await using var stream = file.OpenReadStream();
        await blobClient.UploadAsync(stream, overwrite: true, cancellationToken: cancellationToken);

        return blobClient.Uri.ToString();
    }

    public async Task RemoveImageFromAzureBlob(string name, string containerName)
    {
        var container = serviceClient.GetBlobContainerClient(containerName);
        if (!await container.ExistsAsync()) 
            throw new EmAppException("Brak kontenera o takiej nazwie na Azure","NO_CONTAINER_NAME_IN_AZURE");
        
        var blobClient = container.GetBlobClient(name);
        await blobClient.DeleteIfExistsAsync();
    }
}