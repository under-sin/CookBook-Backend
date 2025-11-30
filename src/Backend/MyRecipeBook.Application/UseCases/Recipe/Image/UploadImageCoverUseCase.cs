using FileTypeChecker.Extensions;
using FileTypeChecker.Types;
using Microsoft.AspNetCore.Http;
using MyRecipeBook.Domain.Extensions;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.Recipes;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Domain.Services.Storage;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;

namespace MyRecipeBook.Application.UseCases.Recipe.Image;

public class UploadImageCoverUseCase(
    IRecipeUpdateOnlyRepository repository,
    ILoggedUser loggedUser,
    IUnitOfWork unitOfWork,
    IBlobStorageService blobStorageService) : IUploadImageCoverUseCase
{
    public async Task Execute(long recipeId, IFormFile file)
    {
        var fileStream = file.OpenReadStream();

        if (fileStream.Is<PortableNetworkGraphic>().IsFalse() 
            && fileStream.Is<JointPhotographicExpertsGroup>().IsFalse())
        {
            throw new ErrorOnValidationException([ResourceMessagesException.INVALID_IMAGE_FORMAT]);
        }

        var user = await loggedUser.User();

        var recipe = await repository.GetById(user, recipeId);

        if (recipe is null)
            throw new NotFoundException(ResourceMessagesException.RECIPE_NOT_FOUND);

        if (string.IsNullOrEmpty(recipe.ImageIdentifier))
        {
            recipe.ImageIdentifier = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            
            repository.Update(recipe);
            
            await unitOfWork.Commit();
        }
        
        // Reset stream position before upload
        fileStream.Position = 0;
        
        await blobStorageService.Upload(user, fileStream, recipe.ImageIdentifier);
    }
}