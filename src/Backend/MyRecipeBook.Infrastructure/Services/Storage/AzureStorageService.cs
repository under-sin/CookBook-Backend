using Azure.Storage.Blobs;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Services.Storage;

namespace MyRecipeBook.Infrastructure.Services.Storage;

public class AzureStorageService(BlobServiceClient blobServiceClient) : IBlobStorageService
{
    public async Task Upload(User user, Stream stream, string fileName)
    {
        var containerClient = blobServiceClient.GetBlobContainerClient(user.UserIdentifier.ToString());
        await containerClient.CreateIfNotExistsAsync();
        
        var blobClient = containerClient.GetBlobClient(fileName);
        
        // Save the file, overwriting if it already exists
        await blobClient.UploadAsync(stream, overwrite: true);
    }
}