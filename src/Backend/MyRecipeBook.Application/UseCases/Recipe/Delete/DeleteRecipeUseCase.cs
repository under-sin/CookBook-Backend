
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.Recipes;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Domain.Services.Storage;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;

namespace MyRecipeBook.Application.UseCases.Recipe.Delete;

public class DeleteRecipeUseCase(
    IRecipeWriteOnlyRepository writeOnlyRepository,
    IRecipeReadOnlyRepository readOnlyRepository,
    ILoggedUser user,
    IUnitOfWork unitOfWork,
    IBlobStorageService blobStorageService) : IDeleteRecipeUseCase
{
    public async Task Execute(long recipeId)
    {
        var loggedUser = await user.User();
        var recipe = await readOnlyRepository.GetById(loggedUser, recipeId);

        if (recipe is null)
            throw new NotFoundException(ResourceMessagesException.RECIPE_NOT_FOUND);

        if (!string.IsNullOrEmpty(recipe.ImageIdentifier))
            await blobStorageService.DeleteFile(loggedUser, recipe.ImageIdentifier);
        
        await writeOnlyRepository.Delete(recipeId);
        await unitOfWork.Commit();
    }
}
