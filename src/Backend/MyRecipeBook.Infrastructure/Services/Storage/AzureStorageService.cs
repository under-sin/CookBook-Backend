using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Extensions;
using MyRecipeBook.Domain.Services.Storage;
using MyRecipeBook.Domain.ValueObjects;

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

    public async Task<string> GetImageUrl(User user, string fileName)
    {
        var containerName = user.UserIdentifier.ToString();
        
        var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
        var exists = await containerClient.ExistsAsync();
        if (exists.Value.IsFalse())
            return string.Empty;
        
        var blobClient = containerClient.GetBlobClient(fileName);
        exists = await blobClient.ExistsAsync();
        if (!exists.Value) 
            return string.Empty;
        
        // set up the SAS token with read permissions and expiration time
        var sasBuilder = new BlobSasBuilder
        {
            BlobContainerName = containerName,
            BlobName = fileName,
            Resource = "b", // b indicates a blob resource
            ExpiresOn = DateTimeOffset.UtcNow.AddMinutes(MyRecipeBookRuleConstants.MaximiumImageUrlLifeTimeInMinutes)
        };
            
        sasBuilder.SetPermissions(BlobSasPermissions.Read);

        return blobClient.GenerateSasUri(sasBuilder).ToString();
    }

    public async Task DeleteFile(User user, string fileName)
    {
        var containerClient = blobServiceClient.GetBlobContainerClient(user.UserIdentifier.ToString());
        var exists = await containerClient.ExistsAsync();
        if (exists.Value)
        {
            await containerClient.DeleteBlobIfExistsAsync(fileName);
        }
    }

    public async Task DeleteContainer(Guid userIdentifier)
    {
        var containerClient = blobServiceClient.GetBlobContainerClient(userIdentifier.ToString());
        await containerClient.DeleteIfExistsAsync();
    }
}