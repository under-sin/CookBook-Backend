using MyRecipeBook.Domain.Entities;

namespace MyRecipeBook.Domain.Repositories.Recipes;

public interface IRecipeWriteOnlyRepository
{
    Task Add(Recipe recipe);
    Task Delete(long recipeId);
}