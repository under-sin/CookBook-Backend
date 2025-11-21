using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;

namespace MyRecipeBook.Application.UseCases.Recipe.Generate;

public interface IGenerateRecipeUseCase
{
    Task<ResponseGenerateRecipeJson> Execute(RequestGenerateRecipeJson request);
}