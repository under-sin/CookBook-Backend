using FileTypeChecker.Extensions;
using FileTypeChecker.Types;
using Microsoft.AspNetCore.Http;
using MyRecipeBook.Application.Extensions;
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
        var user = await loggedUser.User();

        var recipe = await repository.GetById(user, recipeId);

        if (recipe is null)
            throw new NotFoundException(ResourceMessagesException.RECIPE_NOT_FOUND);
        
        var fileStream = file.OpenReadStream();

        var (isValidImage, extension) = fileStream.ValidateAndGetImageExtension();
        
        if (isValidImage.IsFalse())
            throw new ErrorOnValidationException([ResourceMessagesException.INVALID_IMAGE_FORMAT]);

        if (string.IsNullOrEmpty(recipe.ImageIdentifier))
        {
            recipe.ImageIdentifier = $"{Guid.NewGuid()}{extension}";
            
            repository.Update(recipe);
            
            await unitOfWork.Commit();
        }
        
        await blobStorageService.Upload(user, fileStream, recipe.ImageIdentifier);
    }
}