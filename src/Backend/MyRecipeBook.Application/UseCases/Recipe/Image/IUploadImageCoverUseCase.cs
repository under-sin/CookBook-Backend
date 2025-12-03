using Microsoft.AspNetCore.Http;

namespace MyRecipeBook.Application.UseCases.Recipe.Image;

public interface IUploadImageCoverUseCase
{
    Task Execute(long recipeId, IFormFile file);
}