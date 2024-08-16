using System;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Services.LoggedUser;

namespace MyRecipeBook.Application.UseCases.User.Profile.GetUserProfile;

public class GetUserProfileUserCase : IGetUserProfileUserCase
{
    private readonly ILoggedUser _loggedUser;

    public GetUserProfileUserCase(ILoggedUser loggedUser)
    {
        _loggedUser = loggedUser;
    }

    public async Task<ResponseUserProfileJson> Execute()
    {
        var user = await _loggedUser.User();

        return new ResponseUserProfileJson
        {
            Email = user.Email,
            Name = user.Name
        };
    }
}
