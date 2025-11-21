using MyRecipeBook.Communication.Enums;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Extensions;
using MyRecipeBook.Domain.Services.OpenAI;
using MyRecipeBook.Exceptions.ExceptionsBase;

namespace MyRecipeBook.Application.UseCases.Recipe.Generate;

public class GenerateRecipeUseCase(IGenerateRecipeAI generetor) : IGenerateRecipeUseCase
{
    public async Task<ResponseGenerateRecipeJson> Execute(RequestGenerateRecipeJson request)
    {
        Validate(request);
        
        var response = await generetor.Generate(request.Ingredients);
        
        return new  ResponseGenerateRecipeJson 
        {
            Title = response.Title,
            Ingredients = response.Ingredients,
            CookingTime = (CookingTime)response.CookingTime,
            Instructions = response.Instructions.Select(c => new ResponseGenerateInstructionJson
            {
                Step = c.Step,
                Text = c.Text
            }).ToList(),
            Difficulty = Difficulty.Low
        };
    }
    
    private static void Validate(RequestGenerateRecipeJson request)
    {
        var result = new GenerateRecipeValidator().Validate(request);
        if (result.IsValid.IsFalse())
            throw new ErrorOnValidationException(result.Errors.Select(e => e.ErrorMessage).Distinct().ToList());
    }
}