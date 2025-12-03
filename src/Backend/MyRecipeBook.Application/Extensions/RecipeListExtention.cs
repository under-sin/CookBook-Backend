using AutoMapper;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Services.Storage;

namespace MyRecipeBook.Application.Extensions;

public static class RecipeListExtention
{
    public static async Task<IList<ResponseShortRecipeJson>> MapToShortRecipeJson(
        this IList<Recipe> recipes,
        User user,
        IBlobStorageService blobStorageService,
        IMapper mapper)
    {
        // Map each recipe to ResponseShortRecipeJson and fetch image URLs asynchronously
        // to improve performance when dealing with multiple recipes.
        var result = recipes.Select(async recipe =>
        {
            var response = mapper.Map<ResponseShortRecipeJson>(recipe);

            if (!string.IsNullOrEmpty(recipe.ImageIdentifier))
            {
                response.ImageUrl = await blobStorageService.GetImageUrl(user, recipe.ImageIdentifier);
            }
            
            return response;
        });

        var response = await Task.WhenAll(result);
        return response;
    }
}