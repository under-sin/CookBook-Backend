using Bogus;
using CommonTestUtilities.FormFiles;
using Microsoft.AspNetCore.Http;
using MyRecipeBook.Communication.Enums;
using MyRecipeBook.Communication.Requests;

namespace CommonTestUtilities.Requests;

public static class RequestRegisterRecipeFormDataBuilder
{
    public static RequestRegisterRecipeFormData Build(IFormFile? image = null)
    {
        var step = 1;
        
        var request = new Faker<RequestRegisterRecipeFormData>()
            .RuleFor(recipe => recipe.Title, f => f.Lorem.Word())
            .RuleFor(recipe => recipe.CookingTime, f => f.PickRandom<CookingTime>())
            .RuleFor(recipe => recipe.Difficulty, f => f.PickRandom<Difficulty>())
            .RuleFor(recipe => recipe.Ingredients, f => f.Make(3, () => f.Commerce.ProductName()))
            .RuleFor(recipe => recipe.DishTypes, f => f.Make(3, f.PickRandom<DishType>))
            .RuleFor(recipe => recipe.Instructions, f => f.Make(3, () => new RequestInstructionJson
            {
                Text = f.Lorem.Paragraph(),
                Step = step++
            }))
            .RuleFor(recipe => recipe.Image, _ => image);
        
        return request;
    }
    
    public static RequestRegisterRecipeFormData BuildWithPngImage()
    {
        return Build(FormFileBuilder.BuildValidPngImage());
    }
    
    public static RequestRegisterRecipeFormData BuildWithJpegImage()
    {
        return Build(FormFileBuilder.BuildValidJpegImage());
    }
    
    public static RequestRegisterRecipeFormData BuildWithInvalidImage()
    {
        return Build(FormFileBuilder.BuildInvalidImage());
    }
}
