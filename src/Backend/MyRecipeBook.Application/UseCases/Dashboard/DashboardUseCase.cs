using AutoMapper;
using MyRecipeBook.Application.Extensions;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Repositories.Recipes;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Domain.Services.Storage;

namespace MyRecipeBook.Application.UseCases.Dashboard;

public class DashboardUseCase(
    IRecipeReadOnlyRepository repository,
    IMapper mapper,
    ILoggedUser loggedUser,
    IBlobStorageService blobStorageService) : IDashboardUseCase
{
    public async Task<ResponseRecipesJson> Execute()
    {
        var user = await loggedUser.User();
        var recipes = await repository.GetDashboards(user);
        
        return new ResponseRecipesJson()
        {
            Recipes = await recipes.MapToShortRecipeJson(user, blobStorageService, mapper)
        };
    }
}