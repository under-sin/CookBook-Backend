using Microsoft.EntityFrameworkCore;
using MyRecipeBook.Domain.Dtos;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Extensions;
using MyRecipeBook.Domain.Repositories.Recipes;

namespace MyRecipeBook.Infrastructure.DataAccess.Repositories;

public class RecipeRepository(MyRecipeBookDbContext context) : IRecipeWriteOnlyRepository, IRecipeReadOnlyRepository
{
    public async Task Add(Recipe recipe) => await context.Recipes.AddAsync(recipe);

    public async Task<IList<Recipe>> Filter(User user, FilterRecipeDto filter)
    {
        var query = context
            .Recipes
            .AsNoTracking()
            .Include(recipe => recipe.Ingredients)
            .Where(recipe => recipe.Active && recipe.UserId == user.Id);

        if (filter.Difficulties.Any())
        {
            query = query.Where(recipe => recipe.Difficulty.HasValue 
                && filter.Difficulties.Contains(recipe.Difficulty.Value));
        }

        if (filter.CookingTimes.Any())
        {
            query = query.Where(recipe => recipe.CookingTime.HasValue 
                && filter.CookingTimes.Contains(recipe.CookingTime.Value));
        }

        if (filter.DishTypes.Any())
        {
            query = query.Where(recipe => recipe.DishTypes.Any(dishType => filter.DishTypes.Contains(dishType.Type)));
        }

        if (filter.RecipeTitle_Ingredient.NotEmpty())
        {
            query = query.Where(recipe => recipe.Title.Contains(filter.RecipeTitle_Ingredient)
                || recipe.Ingredients.Any(ingredient => ingredient.Item.Contains(filter.RecipeTitle_Ingredient)));
        }

        return await query.ToListAsync();
    }

    public async Task<Recipe?> GetById(User user, long recipeId)
    {
        return await context
            .Recipes
            .AsNoTracking()
            .Include(r => r.Ingredients)
            .Include(r => r.Instructions)
            .Include(r => r.DishTypes)
            .FirstOrDefaultAsync(r => r.Active && r.Id == recipeId && r.UserId == user.Id);
    }
}