using AutoMapper;
using MyRecipeBook.Application.Extensions;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Extensions;
using MyRecipeBook.Domain.Repositories.Recipes;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Domain.Services.Storage;
using MyRecipeBook.Exceptions.ExceptionsBase;

namespace MyRecipeBook.Application.UseCases.Recipe.Filter;

public class FilterRecipeUseCase(
    IRecipeReadOnlyRepository repository, 
    IMapper mapper, 
    ILoggedUser loggerUser,
    IBlobStorageService blobStorageService) : IFilterRecipeUseCase
{
    public async Task<ResponseFilterRecipeJson> Execute(RequestFilterRecipeJson request)
    {
        Validate(request);

        var loggedUser = await loggerUser.User();

        var filters = new Domain.Dtos.FilterRecipeDto
        {
            RecipeTitleIngredient = request.RecipeTitle_Ingredient,
            CookingTimes = request.CookingTimes.Distinct().Select(c => (Domain.Enums.CookingTime)c).ToList(),
            Difficulties = request.Difficulties.Distinct().Select(c => (Domain.Enums.Difficulty)c).ToList(),
            DishTypes = request.DishTypes.Distinct().Select(c => (Domain.Enums.DishType)c).ToList(),
        };

        var result = await repository.Filter(loggedUser, filters);

        return new ResponseFilterRecipeJson
        {
            Recipes = await result.MapToShortRecipeJson(loggedUser, blobStorageService, mapper)
        };
    }

    private static void Validate(RequestFilterRecipeJson request)
    {
        var result = new FilterRecipeValidator().Validate(request);
        if (result.IsValid.IsFalse())
            throw new ErrorOnValidationException(result.Errors.Select(e => e.ErrorMessage).Distinct().ToList());
    }
}
