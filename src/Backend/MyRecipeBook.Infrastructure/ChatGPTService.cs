using MyRecipeBook.Domain.Dtos;
using MyRecipeBook.Domain.Services.OpenAI;

namespace MyRecipeBook.Infrastructure;

public class ChatGPTService : IGenerateRecipeAI
{
    public Task<GeneratedRecipeDto> Generate(IList<string> ingredients)
    {
        throw new NotImplementedException();
    }
}