using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;

namespace MyRecipeBook.Application.UseCases.User.Profile.UpdateUserProfile;

public interface IUpdateUserProfileUseCase
{
    public Task<ResponseUserProfileJson> Execute(RequestUpdateUserJson request);
}