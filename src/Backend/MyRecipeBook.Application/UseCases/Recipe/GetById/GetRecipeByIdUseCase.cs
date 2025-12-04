using AutoMapper;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Repositories.Recipes;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Domain.Services.Storage;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;

namespace MyRecipeBook.Application.UseCases.Recipe.GetById;

public class GetRecipeByIdUseCase(
    ILoggedUser loggedUser, 
    IMapper mapper, 
    IRecipeReadOnlyRepository repository,
    IBlobStorageService blobStorageService) : IGetRecipeByIdUseCase
{
    public async Task<ResponseRecipeJson> Execute(long recipeId)
    {
        var user = await loggedUser.User();

        var recipe = await repository.GetById(user, recipeId);

        if (recipe is null)
            throw new NotFoundException(ResourceMessagesException.RECIPE_NOT_FOUND);

        var response = mapper.Map<ResponseRecipeJson>(recipe);

        if (!string.IsNullOrEmpty(recipe.ImageIdentifier))
        {
            var url = await blobStorageService.GetImageUrl(user, recipe.ImageIdentifier);
            response.ImageUrl = url;
        }
        
        return response;
    }
}
