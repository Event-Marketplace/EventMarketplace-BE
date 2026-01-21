using EventMarketplace.Application.Utils.Azure;
using Microsoft.AspNetCore.Http;

namespace EventMarketplace.Application.Utils;

public class EventFileUploader(IBlobStorageService blobStorageService)
{
    private const string ContainerName = "events";
    public async Task<string> UploadEventImageAsync(IFormFile file, CancellationToken cancellationToken)
    {
        return await blobStorageService.UploadFileAsync(file, ContainerName, cancellationToken);
    }

    public async Task DeleteEventImageAsync(string imageUrl)
    {
        var imageName = imageUrl.Split('/').Last();
        await blobStorageService.RemoveImageFromAzureBlob($"{imageName}", ContainerName);
    }

    public async Task<string> UploadOrReplaceFileAsync(string imageUrl, IFormFile file, CancellationToken cancellationToken)
    {
        string imageName;
            
        if (!string.IsNullOrWhiteSpace(imageUrl))
        {
            try
            {
                var uri = new Uri(imageUrl);
                imageName = Path.GetFileName(uri.AbsolutePath);
            }
            catch
            {
                imageName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            }
        }
        else
        {
            imageName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        }

        var newUri = await blobStorageService.UploadOrReplaceFileAsync(file, imageName, ContainerName,
            cancellationToken);

        return newUri;
    }
}