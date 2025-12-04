using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using MyRecipeBook.Domain.Dtos;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Extensions;
using MyRecipeBook.Domain.Repositories.Recipes;
using MyRecipeBook.Domain.Repositories.Users;

namespace MyRecipeBook.Infrastructure.DataAccess.Repositories;

public class RecipeRepository(MyRecipeBookDbContext context)
    : IRecipeWriteOnlyRepository, IRecipeReadOnlyRepository, IRecipeUpdateOnlyRepository, IDeleteUserOnlyRepository
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

        if (filter.RecipeTitleIngredient.NotEmpty())
        {
            query = query.Where(recipe => recipe.Title.Contains(filter.RecipeTitleIngredient)
                || recipe.Ingredients.Any(ingredient => ingredient.Item.Contains(filter.RecipeTitleIngredient)));
        }

        return await query.ToListAsync();
    }

    public async Task Delete(long recipeId)
    {
        // Nao é necessário verificar se a receita existe, pois isso já foi feito no UseCase
        var recipe = await context.Recipes.FindAsync(recipeId);
        context.Recipes.Remove(recipe!);
    }

    // Implementação explícita para evitar conflitos de método entre as interfaces
    async Task<Recipe?> IRecipeReadOnlyRepository.GetById(User user, long recipeId)
    {
        return await GetFullRecipe()
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.Active && r.Id == recipeId && r.UserId == user.Id);
    }

    public async Task<IList<Recipe>> GetDashboards(User user)
    {
        return await context
            .Recipes
            .AsNoTracking()
            .Include(x => x.Ingredients)
            .Where(r => r.Active && r.UserId == user.Id)
            .OrderByDescending(r => r.CreatedOn)
            .Take(5)
            .ToListAsync();
    }

    async Task<Recipe?> IRecipeUpdateOnlyRepository.GetById(User user, long recipeId)
    {
        return await GetFullRecipe()
            .FirstOrDefaultAsync(r => r.Active && r.Id == recipeId && r.UserId == user.Id);
    }

    public void Update(Recipe recipe)
    {
        context.Update(recipe);
    }

    private IIncludableQueryable<Recipe, IList<DishType>> GetFullRecipe()
    {
        return context
            .Recipes
            .Include(r => r.Ingredients)
            .Include(r => r.Instructions)
            .Include(r => r.DishTypes);
    }

    public async Task DeleteAccount(Guid userIdentifier)
    {
        var user = await context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.UserIdentifier == userIdentifier);
        if (user == null)
            return;
        
        var recipes = context.Recipes.Where(x => x.UserId == user.Id);
        context.Recipes.RemoveRange(recipes);
        
        context.Users.Remove(user);
    }
}