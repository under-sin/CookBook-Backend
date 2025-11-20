using AutoMapper;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Repositories.Recipes;
using MyRecipeBook.Domain.Services.LoggedUser;

namespace MyRecipeBook.Application.UseCases.Dashboard;

public class DashboardUseCase(
    IRecipeReadOnlyRepository repository,
    IMapper mapper,
    ILoggedUser loggedUser) : IDashboardUseCase
{
    public async Task<ResponseRecipesJson> Execute()
    {
        var user = await loggedUser.User();
        var recipes = await repository.GetDashboards(user);
        
        return new ResponseRecipesJson()
        {
            Recipes = mapper.Map<IList<ResponseShortRecipeJson>>(recipes)
        };
    }
}