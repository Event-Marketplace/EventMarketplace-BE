using EventMarketplace.Application.Utils.Azure;
using Microsoft.AspNetCore.Http;

namespace EventMarketplace.Application.Utils;

public class EventFileUploader(IBlobStorageService blobStorageService)
{
    private string ContainerName { get; } = "events";
    public async Task<string> UploadEventImageAsync(IFormFile file, CancellationToken cancellationToken)
    {
        return await blobStorageService.UploadFileAsync(file, ContainerName, cancellationToken);
    }

    public async Task DeleteEventImageAsync(string imageUrl)
    {
        var imageName = imageUrl.Split('/').Last();
        await blobStorageService.RemoveImageFromAzureBlob($"{imageName}", "events");
    }
}