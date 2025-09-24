using Bogus;
using MyRecipeBook.Communication.Enums;
using MyRecipeBook.Communication.Requests;

namespace CommonTestUtilities.Requests;

public static class RequestRecipeJsonBuilder
{
    public static RequestRecipeJson Build()
    {
        return new Faker<RequestRecipeJson>()
            .RuleFor(recipe => recipe.Title, f => f.Lorem.Word())
            .RuleFor(recipe => recipe.CookingTime, f => f.PickRandom<CookingTime>())
            .RuleFor(recipe => recipe.Difficulty, f => f.PickRandom<Difficulty>());
    }
}
