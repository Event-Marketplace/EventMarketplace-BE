using EventMarketplace.Application.Utils.Azure;
using Microsoft.AspNetCore.Http;

namespace EventMarketplace.Application.Utils;

public class EventFileIUploader(IBlobStorageService blobStorageService)
{
    public async Task<string> UploadEventImageAsync(IFormFile file, CancellationToken cancellationToken)
    {
        return await blobStorageService.UploadFileAsync(file, "events", cancellationToken);
    }
}