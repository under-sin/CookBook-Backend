using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Repositories.Recipes;

namespace MyRecipeBook.Infrastructure.DataAccess.Repositories;

public class RecipeRepository(MyRecipeBookDbContext context) : IRecipeWriteOnlyRepository
{
    public async Task Add(Recipe recipe) => await context.Recipes.AddAsync(recipe);
}