using AutoMapper;
using FileTypeChecker.Extensions;
using FileTypeChecker.Types;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Extensions;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.Recipes;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Domain.Services.Storage;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;

namespace MyRecipeBook.Application.UseCases.Recipe.Register;

public class RegisterRecipeUseCase(
    IRecipeWriteOnlyRepository repository,
    IBlobStorageService blobStorageService,
    ILoggedUser loggedUser,
    IUnitOfWork unitOfWork,
    IMapper mapper) : IRegisterRecipeUseCase
{
    public async Task<ResponseRegisteredRecipeJson> Execute(RequestRegisterRecipeFormData request)
    {
        Validate(request);

        var loggedUSer = await loggedUser.User();
        
        var recipe = mapper.Map<Domain.Entities.Recipe>(request);
        recipe.UserId = loggedUSer.Id;
        
        // regra para ordenas os "steps" caso o usuário não passe a numeração sequencial
        var instructions = recipe.Instructions.OrderBy(i => i.Step).ToList();
        for (var index = 0; index < instructions.Count ; index++)
            instructions.ElementAt(index).Step = index + 1;
        
        recipe.Instructions = mapper.Map<IList<Instruction>>(instructions);

        // refatorar essa parte de upload de imagem
        if (request.Image != null)
        {
            recipe.ImageIdentifier = $"{Guid.NewGuid()}{Path.GetExtension(request.Image.FileName)}";
            
            var fileStream = request.Image.OpenReadStream();

            if (fileStream.Is<PortableNetworkGraphic>().IsFalse() 
                && fileStream.Is<JointPhotographicExpertsGroup>().IsFalse())
            {
                throw new ErrorOnValidationException([ResourceMessagesException.INVALID_IMAGE_FORMAT]);
            }
            
            fileStream.Position = 0;
            
            await blobStorageService.Upload(loggedUSer, fileStream, recipe.ImageIdentifier);
        }
        
        await repository.Add(recipe);
        await unitOfWork.Commit();
        
        return mapper.Map<ResponseRegisteredRecipeJson>(recipe);
    }

    private static void Validate(RequestRecipeJson request)
    {
        var result = new RecipeValidator().Validate(request);
        if (result.IsValid.IsFalse())
            throw new ErrorOnValidationException(result.Errors.Select(e => e.ErrorMessage).Distinct().ToList());
    }
}