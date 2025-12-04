using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.Users;
using MyRecipeBook.Domain.Services.Storage;

namespace MyRecipeBook.Application.UseCases.User.Delete.Delete;

public class DeleteUserAccountUseCase(
    IDeleteUserOnlyRepository repository,
    IUnitOfWork unitOfWork,
    IBlobStorageService blobStorageService) : IDeleteUserAccountUseCase
{
    public async Task Execute(Guid userIdentifier)
    {
        await repository.DeleteAccount(userIdentifier);
        await blobStorageService.DeleteContainer(userIdentifier);
        await unitOfWork.Commit();
    }
}