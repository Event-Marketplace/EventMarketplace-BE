using Microsoft.AspNetCore.Http;

namespace EventMarketplace.Application.Utils.Azure;

public interface IBlobStorageService
{
    Task<string> UploadFileAsync(IFormFile file, string containerName, CancellationToken cancellationToken);
    Task<string> UploadOrReplaceFileAsync(IFormFile file, string imageName, string containerName, CancellationToken cancellationToken);
    Task RemoveImageFromAzureBlob(string name, string containerName);
}