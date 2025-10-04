using AutoMapper;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Repositories.Recipes;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;

namespace MyRecipeBook.Application.UseCases.Recipe.GetById;

public class GetRecipeByIdUseCase : IGetRecipeByIdUseCase
{
    private ILoggedUser _loggedUser;
    private IMapper _mapper;
    private IRecipeReadOnlyRepository _repository;

    public GetRecipeByIdUseCase(ILoggedUser loggedUser, IMapper mapper, IRecipeReadOnlyRepository repository)
    {
        _loggedUser = loggedUser;
        _mapper = mapper;
        _repository = repository;
    }

    public async Task<ResponseRecipeJson> Execute(long recipeId)
    {
        var user = await _loggedUser.User();

        var recipe = await _repository.GetById(user, recipeId);

        if (recipe is null)
            throw new NotFoundException(ResourceMessagesException.RECIPE_NOT_FOUND);

        return _mapper.Map<ResponseRecipeJson>(recipe);
    }
}
