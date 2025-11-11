using Microsoft.AspNetCore.Http;

namespace EventMarketplace.Application.Utils.Azure;

public interface IBlobStorageService
{
    Task<string> UploadFileAsync(IFormFile file, string containerName, CancellationToken cancellationToken);
}