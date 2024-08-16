using System;
using MyRecipeBook.Communication.Responses;

namespace MyRecipeBook.Application.UseCases.User.Profile.GetUserProfile;

public interface IGetUserProfileUserCase
{
    Task<ResponseUserProfileJson> Execute();
}
