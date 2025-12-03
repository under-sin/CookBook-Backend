using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Extensions;
using MyRecipeBook.Domain.Repositories.Users;
using MyRecipeBook.Domain.Security.Cryptography;
using MyRecipeBook.Domain.Security.Tokens;
using MyRecipeBook.Exceptions.ExceptionsBase;

namespace MyRecipeBook.Application.UseCases.Login.DoLogin;

public class DoLoginUseCase(
    IUserReadOnlyRepository userRepository,
    IAccessTokenGenerator accessTokenGenerator,
    IPasswordEncripter passwordEncripter) : IDoLoginUseCase
{
    public async Task<ResponseRegisterUserJson> Execute(RequestLoginJson request)
    {
        var user = await userRepository.GetByEmail(request.Email);
        if (user == null || passwordEncripter.IsValid(request.Password, user.Password).IsFalse())
            throw new InvalidLoginException();

        return new ResponseRegisterUserJson
        {
            Name = user.Name,
            Tokens = new ResponseTokensJson
            {
                AccessToken = accessTokenGenerator.Generator(user.UserIdentifier)
            }
        };
    }
}