using MyRecipeBook.Domain.Entities;

namespace MyRecipeBook.Domain.Repositories.Recipes;

public interface IRecipeUpdateOnlyRepository
{
    Task<Recipe?> GetById(User user, long recipeId);
    void Update(Recipe recipe);
}
