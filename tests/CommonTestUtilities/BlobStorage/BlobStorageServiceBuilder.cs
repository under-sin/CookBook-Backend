using Bogus;
using Moq;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Services.Storage;

namespace CommonTestUtilities.BlobStorage;

public class BlobStorageServiceBuilder
{

    private readonly Mock<IBlobStorageService> _mock;
    
    public BlobStorageServiceBuilder()
    {
        _mock = new Mock<IBlobStorageService>();
        
        _mock.Setup(blobStorage => blobStorage.Upload(It.IsAny<User>(), It.IsAny<Stream>(), It.IsAny<string>()))
            .Returns(Task.CompletedTask);
    }

    public BlobStorageServiceBuilder GetImageUrl(User user, string? fileName)
    {
        if (string.IsNullOrEmpty(fileName))
            return this;

        var faker = new Faker();
        var imageUrl = faker.Image.LoremPixelUrl();

        _mock.Setup(blobStorage => blobStorage.GetImageUrl(user, fileName)).ReturnsAsync(imageUrl);

        return this;
    }
    
    public BlobStorageServiceBuilder GetImageUrl(User user, IList<Recipe> recipes)
    {
        foreach (var recipe in recipes)
        {
            GetImageUrl(user, recipe.ImageIdentifier);
        }

        return this;
    }
    
    public IBlobStorageService Build() => _mock.Object;
}