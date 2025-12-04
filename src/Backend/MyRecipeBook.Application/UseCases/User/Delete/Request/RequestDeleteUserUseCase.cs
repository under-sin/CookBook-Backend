using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.Users;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Domain.Services.ServiceBus;

namespace MyRecipeBook.Application.UseCases.User.Delete.Request;

public class RequestDeleteUserUseCase(
    IUserUpdateOnlyRepository updateOnlyRepository,
    IUnitOfWork unitOfWork,
    ILoggedUser loggedUser,
    IDeleteUserQueue queue) : IRequestDeleteUserUseCase
{
    public async Task Execute()
    {
        var logUser = await loggedUser.User();
        var user = await updateOnlyRepository.GetByIdAsync(logUser.Id);

        user.Active = false;
        updateOnlyRepository.Update(user);

        await unitOfWork.Commit();
        
        await queue.SendMessage(user);
    }
}