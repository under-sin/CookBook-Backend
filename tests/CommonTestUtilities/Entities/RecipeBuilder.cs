using Bogus;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Enums;

namespace CommonTestUtilities.Entities;

public static class RecipeBuilder
{
    public static IList<Recipe> Collection(User user, uint count = 3)
    {
        var list = new List<Recipe>();

        if (count == 0)
            count = 1;

        var recipeId = 1;

        for (int i = 0; i < count; i++) {
            var recipe = Build(user);
            recipe.Id = recipeId++;

            list.Add(recipe);
        }

        return list;
    }

    public static Recipe Build(User user)
    {
        return new Faker<Recipe>()
            .RuleFor(r => r.Id, () => 1)
            .RuleFor(recipe => recipe.Title, f => f.Lorem.Word())
            .RuleFor(recipe => recipe.CookingTime, f => f.PickRandom<CookingTime>())
            .RuleFor(recipe => recipe.Difficulty, f => f.PickRandom<Difficulty>())
            .RuleFor(recipe => recipe.ImageIdentifier, _ => $"{Guid.NewGuid()}.jpg")
            .RuleFor(recipe => recipe.Ingredients, f => f.Make(1, () => new Ingredient
            {
                Id = 1,
                Item = f.Commerce.ProductName()
            }))
            .RuleFor(recipe => recipe.Instructions, f => f.Make(1, () => new Instruction
            {
                Id = 1,
                Step = 1,
                Text = f.Lorem.Paragraph()
            }))
            .RuleFor(recipe => recipe.DishTypes, f => f.Make(1, () => new MyRecipeBook.Domain.Entities.DishType
            {
                Id = 1,
                Type = f.PickRandom<MyRecipeBook.Domain.Enums.DishType>()
            }))
            .RuleFor(r => r.UserId, () => user.Id);
    }
}
