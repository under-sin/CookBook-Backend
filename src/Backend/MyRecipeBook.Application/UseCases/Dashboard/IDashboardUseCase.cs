using MyRecipeBook.Communication.Responses;

namespace MyRecipeBook.Application.UseCases.Dashboard;

public interface IDashboardUseCase
{
    Task<ResponseRecipesJson> Execute();
}